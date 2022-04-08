
namespace DataSynchronizationService.DAL.SqlTableDefinitions
{
    public static class ProfilesExtentions
    {
        public static object GetPropValue(this object o, string propName)
        {
            return o.GetType().GetProperty(propName).GetValue(o, null);
        }
    }
}
