//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Connect.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.EmailVerifications = new HashSet<VerifyEmail>();
            this.Records = new HashSet<Record>();
            this.Sender = new HashSet<Connection>();
            this.Receiver = new HashSet<Connection>();
            this.From = new HashSet<Message>();
            this.To = new HashSet<Message>();
            this.Educations = new HashSet<Education>();
            this.WorkXp = new HashSet<WorkXp>();
        }
    
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Nullable<bool> IsEmailVerified { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> RegisteredDateTime { get; set; }
        public Nullable<System.Guid> registeredUniqueId { get; set; }
        public Nullable<bool> IsUserVerified { get; set; }
        public Nullable<System.Guid> VerifiedUniqueId { get; set; }
        public Nullable<System.DateTime> VerifiedDateTime { get; set; }
        public Nullable<int> LoginRetryAttempts { get; set; }
        public Nullable<bool> IsAccountLocked { get; set; }
        public Nullable<System.DateTime> LockDateTime { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string City { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VerifyEmail> EmailVerifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Record> Records { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Connection> Sender { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Connection> Receiver { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> From { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> To { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Education> Educations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkXp> WorkXp { get; set; }
    }
}
