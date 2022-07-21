namespace Application.Manager
{
    public class UserManager : DomainManagerBase<User>, IUserManager
    {
        public UserManager(DataStoreContext storeContext) : base(storeContext)
        {

        }

        public async Task<bool> ChangePasswordAsync(User user, string newPassword)
        {
            user!.PasswordSalt = HashCrypto.BuildSalt();
            user.PasswordHash = HashCrypto.GeneratePwd(newPassword, user.PasswordSalt);
            return await SaveChangesAsync() > 0;
        }

        public override async Task<User> UpdateAsync(Guid id, User entity)
        {
            entity.PasswordSalt = HashCrypto.BuildSalt();
            entity.PasswordHash = HashCrypto.GeneratePwd(entity.PasswordHash, entity.PasswordSalt);
            await base.UpdateAsync(id, entity);
            await SaveChangesAsync();
            return entity;
        }

    }
}
