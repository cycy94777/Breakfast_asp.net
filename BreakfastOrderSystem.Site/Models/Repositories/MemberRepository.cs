using BreakfastOrderSystem.Site.Models.EFModels;
using BreakfastOrderSystem.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreakfastOrderSystem.Site.Models.Repositories
{
    public class MemberRepository
    {
       
        private AppDbContext _db;

        public MemberRepository()
        {
            _db = new AppDbContext();
        }

        public IEnumerable<Member> GetMembers()
        {
            var memberData = _db.Members;
            return memberData.ToList();
        }

        public IEnumerable<PointDetail> GetPointDetail()
        {
            var pointDetail = _db.PointDetails;
            return pointDetail.ToList();
        }

        public Member GetMember(string memberName)
        {
            return _db.Members.FirstOrDefault(m => m.Name == memberName);
        }

        // 獲得被列進黑名單的會員
        public IEnumerable<Member> GetBlacklistedMembers()
        {
            return _db.Members
                           .Where(m => m.BlackList)
                           .ToList();
        }

        // 將指定會員移出黑名單
        public void UnblockMember(string account)
        {
            var member = _db.Members.FirstOrDefault(m => m.Account == account);
            if (member != null)
            {
                member.BlackList = false;
                _db.SaveChanges();
            }
        }



    }
}