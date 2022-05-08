using System.Linq;
using MongoDB.Bson;
using ProjectX.DataAccess.Models.Base;
using ProjectX.DataAccess.Models.Identity;
using ProjectX.DataAccess.Repositories.Base;
using static ProjectX.Core.ArgumentRule.ArgumentRule;

namespace ProjectX.BusinessLayer.Services
{
    public class BusinessUserService
    {
        private readonly IRepository<BusinessUser> _repository;

        public BusinessUserService(IRepository<BusinessUser> repository)
        {
            _repository = repository;
        }

        public void CreateBusinessUser(User user)
        {
            NotNull(user, nameof(user));
            
            var businessUser = new BusinessUser
            {
                Email = user.Email,
                UserId = user.Id
            };

            _repository.Insert(businessUser);
        }

        public BusinessUser FindById(ObjectId id)
        {
            return _repository.AsQueryable().Single(user => user.UserId == id);
        }
    }
}