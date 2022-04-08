using System;

namespace DataSynchronizationService.DAL.SqlTableDefinitions
{
    public class UserProfiles
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public bool UseNickName { get; set; }
        public string NickName { get; set; }
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public int? Country { get; set; }
        public int? Region { get; set; }
        public int? City { get; set; }
        public int? Specialization { get; set; }
        public bool UseRegistrationEmail { get; set; }
        public int WhoLookProfile { get; set; }
        public int WhoWriteMessage { get; set; }
        public int WhoLookContactInfo { get; set; }
        public int WhoLookPersonalInfo { get; set; }
        public int LookAdvertisment { get; set; }
        public bool NoticeNewMessage { get; set; }
        public bool NoticeAddCollegua { get; set; }
        public bool NoticePublicComment { get; set; }
        public bool NoticeReplyComment { get; set; }
        public bool NoticeColleagueTopic { get; set; }
        public bool NoticeColleagueBirthday { get; set; }
        public bool NoticeBirthday { get; set; }
        public bool NoticeHospital { get; set; }
        public bool NoticeGroupTopic { get; set; }
        public bool NoticeGroupsRequests { get; set; }
        public bool NoticeBlogTopic { get; set; }
        public bool NoticePeriodicallyIfAbsent { get; set; }
        public int InviteCount { get; set; }
        public int? FirstSpecializationId { get; set; }
        public int? SecondSpecializationId { get; set; }
        public int? ThirdSpecializationId { get; set; }
        public bool Working { get; set; }
    }
}
