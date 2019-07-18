using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CheckMonetaRUPaymentRefunds
{
    class MonetaRuRequest
    {
        private StringBuilder _sb = new StringBuilder();

        public string ReportMessage => _sb.ToString();

        public bool FindOperationsListRequest(DateTime dateFrom, DateTime dateTo, int pageNumber = 1, int pageSize = 1000, string username = "", string password = "")
        {
            var httpWebRequest = (HttpWebRequest) WebRequest.Create("https://service.moneta.ru/services");
            httpWebRequest.ContentType = "application/json;charset=UTF-8";
            httpWebRequest.Method = "POST";
            // формируем запрос
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                // формируем класс для серилизации в JSON, см пример в moneta api -> FindOperationsListRequest
                var jsReq = new
                {
                    Envelope = new
                    {
                        Header = new
                        {
                            Security = new
                            {
                                UsernameToken = new
                                {
                                    Username = username,
                                    Password = password
                                }
                            }
                        },
                        Body = new
                        {
                            FindOperationsListRequest = new
                            {
                                pager = new
                                {
                                    pageNumber,
                                    pageSize
                                },
                                filter = new
                                {
                                    dateFrom,
                                    dateTo
                                }
                            }
                        }
                    }
                };
                // переводим в JSON текст
                var jsonReq = JsonConvert.SerializeObject(jsReq);

                streamWriter.Write(jsonReq);
                streamWriter.Flush();
                streamWriter.Close();

            }

            var result = false;

            // получаем ответ
            var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException()))
            {
                var jsonResp = streamReader.ReadToEnd();

                // парсим JSON ответ от монеты
                JObject monetaResp = JObject.Parse(jsonResp);

                var pageCount = monetaResp["Envelope"]["Body"]["FindOperationsListResponse"]["pagesCount"].Value<int>();
                if (pageCount > pageNumber)
                    result = FindOperationsListRequest(dateFrom, dateTo, pageNumber + 1, pageSize);
                
                // если в ответе не присутствует слово isrefund, значит нет операции с возвратом средств, перейти на след страницу результата
                if (!jsonResp.Contains("isrefund"))
                    // если нету, выходим
                    return false;

                // переходим к массиву operation
                List<JToken> resList = monetaResp["Envelope"]["Body"]["FindOperationsListResponse"]["operation"].Children().ToList();

                // пройдемся по списку и ищем те записи у которых есть isrefund
                foreach (JToken jToken in resList)
                {
                    var id = jToken["id"].ToString();
                    List<JToken> attrs = jToken["attribute"].Children().ToList();

                    // запоминаем родительский ИД, для получения доп. инф-ции, она содержит ин-цию в нашей системе
                    var parentid = string.Empty;
                    var isRefund = false;

                    foreach (var attr in attrs)
                    {
                        // нашил запись с возвратом средств
                        if (attr["key"].ToString() == "isrefund")
                        {
                            isRefund = true;
                            result = true;
                            _sb.AppendLine("Найдена операция взврата средств!");
                            _sb.AppendLine($@"Id операции в moneta.ru: https://moneta.ru/operationInfoDetails.htm?operationId={id}&backUrl=desktop.htm");
                        }

                        // запоминаем родительский Ид
                        if (attr["key"].ToString() == "parentid")
                            parentid = attr["value"].ToString();
                    }

                    // если нету родительского ИДб перейдем к след. записи
                    if (!isRefund || string.IsNullOrEmpty(parentid))
                        continue;

                    _sb.AppendLine($@"Родительская операция в moneta.ru: https://moneta.ru/operationInfoDetails.htm?operationId={parentid}&backUrl=desktop.htm");
                    _sb.AppendLine("Попытка получить дополнительную информацию:");
                    // новый JSON запрос в монету, для получения детальной инф-ции
                    if (long.TryParse(parentid, out var res))
                        GetOperationDetailsByIdRequest(res);
                    else
                        _sb.AppendLine("Не удалось получить информацию!");
                }
            }

            return result;
        }

        public void GetOperationDetailsByIdRequest(long operationId, string username = "", string password = "")
        {
            var httpWebRequest = (HttpWebRequest) WebRequest.Create("https://service.moneta.ru/services");
            httpWebRequest.ContentType = "application/json;charset=UTF-8";
            httpWebRequest.Method = "POST";

            // формиуем запрос
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                // формируем класс для серилизации в JSON, см пример в moneta api -> GetOperationDetailsByIdRequest
                var jsReq = new
                {
                    Envelope = new
                    {
                        Header = new
                        {
                            Security = new
                            {
                                UsernameToken = new
                                {
                                    Username = username,
                                    Password = password
                                }
                            }
                        },
                        Body = new
                        {
                            GetOperationDetailsByIdRequest = operationId
                        }
                    }
                };

                // переводим в JSON текст
                var jsonReq = JsonConvert.SerializeObject(jsReq);

                streamWriter.Write(jsonReq);
                streamWriter.Flush();
                streamWriter.Close();
            }

            // получаем ответ
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException()))
            {
                // JSON ответ от монеты
                var jsonResp = streamReader.ReadToEnd();

                JObject monetaResp = JObject.Parse(jsonResp);

                List<JToken> attrs = monetaResp["Envelope"]["Body"]["GetOperationDetailsByIdResponse"]["operation"]["attribute"].Children().ToList();

                foreach (var attr in attrs)
                {
                    try
                    {
                        if (attr["key"].ToString() == "wirepaymentpurpose")
                            _sb.AppendLine(attr["value"].ToString());

                        if (attr["key"].ToString() == "description")
                            _sb.AppendLine(attr["value"].ToString());


                        if (attr["key"].ToString() == "parentid")
                        {
                            _sb.AppendLine($"\nN_Transaction в нашей БД: {attr["value"]}");
                            _sb.AppendLine($"SELECT * FROM dbo.FD_Payments WHERE N_Transaction_Code = {attr["value"]}\n");
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }
    }
}

