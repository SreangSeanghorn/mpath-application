
using MPath.Domain.Core.Interfaces;
using MPath.Domain.EventDatas;
using MPath.Domain.Events;
using MPath.Domain.ValueObjects;
using MPath.SharedKernel.Primitive;

namespace MPath.Domain.Entities
{
    public class User : AggregateRoot<Guid>
    {
        public Guid Id { get; private set; }
        public string UserName { get;  set; }
        public Email Email { get;  set; }
        public string Password { get;  set; }
         public ICollection<Role> Roles { get;  set; } = new List<Role>();
         public RefreshToken? RefreshToken { get;  set; } 

         public ICollection<Patient> Patients { get; private set; } = new List<Patient>();
        
        private User()
        {
        }
        private User(string userName, Email email, string password)
        {
            UserName = userName;
            Email = email;
            Password = password;
        }
        public static User Create(string userName, Email email, string password)
        {
            return new User(userName, email, password);
        }
        public static User Register(string userName, Email email, string password,Role role)
        {
            var user = Create(userName, email, password);
            user.AssignRole(role);
            var userRegisteredEventData = new UserRegisteredEventData(userName, email.Value, role.Id);
            var userRegisteredEvent = new UserRegisteredEvent(userRegisteredEventData);
            user.RaiseDomainEvents(userRegisteredEvent);
            return user;
        }
        public bool VerifyPassword(string password, IPasswordHasher passwordHasher)
        {
            return passwordHasher.VerifyPassword(Password, password);
        }

        public void AssignRole(Role role)
        {
            if (Roles.Any(r => r.Name == role.Name)) return;
            Roles.Add(role);
            RaiseRoleAssignedEvent(role.Name);
        }
        public void RaiseRoleAssignedEvent(string role)
        {
            var roleAssignedData = new AssignRoleEventData(Id, role);
            var roleAssignedEvent = new AssignedRoleEvent(roleAssignedData);
            RaiseDomainEvents(roleAssignedEvent);
        }

        public List<string> GetRoles()
        {
            return Roles.Select(r => r.Name).ToList();
        }
        

        public void SetRefreshToken(string token, DateTime expiryDate)
        {
            RefreshToken = RefreshToken.Create(token, expiryDate);
        }

        public void RevokeRefreshToken()
        {
            RefreshToken?.Revoke();
        }

        public bool HasValidRefreshToken(string token)
        {
            return RefreshToken?.Token == token && RefreshToken?.IsValid() == true;
        }  
        
        public Patient CreatePatient(string name, Email email ,  string phoneNumber,string address, DateTime dob)
        {
            var patient = Patient.Create(name, email, phoneNumber, address, dob);
            ((List<Patient>)Patients).Add(patient);
            var patientCreatedEventData = new CreatedPatientEventData(name, email.Value, phoneNumber, address, dob);
            var patientCreatedEvent = new CreatedPatientEvent(patientCreatedEventData);
            RaiseDomainEvents(patientCreatedEvent);
            return patient;
        }
        public Recommendation CreateRecommendation(string title, string content, bool isCompleted,Guid PatientId)
        {
            var recommendation = Recommendation.Create(title, content, isCompleted, Id);
            var patient = Patients.FirstOrDefault(p => p.Id == PatientId);
            if (patient != null) patient.AddRecommendation(recommendation);
            return recommendation;
        }
        public void MarkRecommendationAsCompleted(Recommendation recommendation)
        {
            recommendation.MarkCompleted();
        }
    }
    
}