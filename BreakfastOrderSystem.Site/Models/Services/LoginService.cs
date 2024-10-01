using BreakfastOrderSystem.Site.Models.Dtos;
using BreakfastOrderSystem.Site.Models.EFModels;
using BreakfastOrderSystem.Site.Models.Infra;
using BreakfastOrderSystem.Site.Models.Repositories;
using BreakfastOrderSystem.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreakfastOrderSystem.Site.Models.Services
{
    public class LoginService
    {
        private StoreRepository _repo;
        private EmailHelper _emailHelper;
        private AppDbContext _db;

        public LoginService()
        {
            _repo = new StoreRepository(); 
            _emailHelper = new EmailHelper();
            _db = new AppDbContext();

        }

        //public LoginService(StoreRepository repo)
        //{
        //    _repo = repo;
        //}

        public StoreVm Get(string account)
        {
            var store = _repo.Get(account);
            if (store == null) return null;

            return new StoreVm
            {
                Id = store.Id,
                Account = store.Account,
                EncryptedPassword = store.EncryptedPassword,
                Name = store.Name
            };
        }


        public StoreVm Get(int id)
        {
            var store = _repo.Get(id);
            if (store == null) return null;

            return new StoreVm
            {
                Id = store.Id,
                Account = store.Account,
                EncryptedPassword = store.EncryptedPassword,
                Name = store.Name
            };
        }

        public Result ValidateLogin(LoginVm vm)
        {
            //找出店家資料
            StoreVm store = Get(vm.Account);
            if (store == null) return Result.Fail("帳號或密碼錯誤");

            // 將密碼雜湊後比對
            string hashPassword = HashUtility.ToSHA256(vm.Password);
            bool isPasswordCorrect = (hashPassword.CompareTo(store.EncryptedPassword) == 0);

            // 回傳結果
            return isPasswordCorrect
                ? Result.Success()
                : Result.Fail("帳號或密碼錯誤");
        }

        public string GetStoreName(LoginVm vm)
        {
            StoreVm store = Get(vm.Account);
            return store.Name;
        }

        internal Result ProcessChangePassword(int memberId, string password)
        {
            //var db = new AppDbContext();

            // 驗證 memberId, confirmCode 是否正確
            var member = _repo.Get(memberId);

            if (member == null) return Result.Fail("找不到會員紀錄"); // 故意不告知確切錯誤原因

            // 更新密碼，並將 confirmCode清空
            //var salt = HashUtility.GetSalt();
            var encryptedPassword = HashUtility.ToSHA256(password);

            //member.EncryptedPassword = encryptedPassword;
            //memberInDb.ConfirmCode = null;
            var store = _db.Stores.FirstOrDefault(m => m.Id == memberId);
            if (store == null)
                return Result.Fail("找不到商店紀錄");

            store.EncryptedPassword = encryptedPassword;
            _db.SaveChanges();

            return Result.Success();
        }

    }
}