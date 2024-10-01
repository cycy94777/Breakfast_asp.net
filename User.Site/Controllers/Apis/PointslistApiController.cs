using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using User.Site.Models.EFModels;
using User.Site.Models.ViewModels;

namespace User.Site.Controllers.Apis
{
    public class PointslistApiController : ApiController
    {
        [HttpGet]
        [Route("api/pointslistapi/all")]
        public IHttpActionResult Get(int memberId)
        {
            var db = new AppDbContext();

            // 查找當前會員最新的剩餘點數
            var latestRemainingPoints = db.PointDetails
                .Where(p => p.MemberId == memberId)
                .OrderByDescending(p => p.Date)
                .Select(p => p.Remaining)
                .FirstOrDefault();


            // 查找當前會員的點數詳細信息
            var pointDetails = db.PointDetails
                .Where(p => p.MemberId == memberId)
                .Select(p => new PointsDetailVm
                {
                    OrderID = p.OrderId.ToString(),
                    Date = p.Date,
                    TotalAmount = p.Original,
                    EarnedPoints = p.Earned,
                    UsedPoints = p.Used,
                    RemainingPoints = p.Remaining
                }).ToList();

            // 組合 ViewModel 返回結果
            var result = new MemberPointsVm
            {
                CurrentPoints = latestRemainingPoints,
                RankList = pointDetails
            };

            return Ok(result);
        }
    }
}
