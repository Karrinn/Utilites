using System;

namespace DataSynchronizationService.DAL.SqlTableDefinitions
{
    public class Users
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastVisitDate { get; set; }
        public int Status { get; set; }
        public int Profile { get; set; }
        public string SocialId { get; set; }
        public int? SocialType { get; set; }
        public int NotificationEmailSended { get; set; }
        public int OldStatus { get; set; }
        public bool EmailChanged { get; set; }
        public int? EmailStatus { get; set; }
        public DateTime? EmailConfirmed { get; set; }
        public int? EmailConfirmedStatus { get; set; }
        public DateTime? DoctorConfirmed { get; set; }
        public int? DoctorConfirmedStatus { get; set; }
        public DateTime? LastVisitFeedDate { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsUseRedesigned { get; set; }
        public int PreregisteredStatus { get; set; }
        public int? LastRegistrationStepComplete { get; set; }
    }
}
