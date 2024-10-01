using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using User.Site.Models.EFModels;
using User.Site.Models.PointRequest;

namespace User.Site.Controllers.Apis
{
    public class PointDetailsApiController : ApiController
    {
        [HttpPost]
        [Route("api/pointdetails/create")]
        public async Task<IHttpActionResult> CreatePointDetails([FromBody] PointDetailRequest pointDetailRequest)
        {
            using (var db = new AppDbContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. 更新 PointDetails 資料表
                        var pointDetail = new PointDetail
                        {
                            MemberId = pointDetailRequest.MemberId,
                            OrderId = pointDetailRequest.OrderId,
                            Original = pointDetailRequest.Original,
                            Used = pointDetailRequest.Used,
                            Earned = pointDetailRequest.Earned,
                            Remaining = pointDetailRequest.Remaining,
                            Date = DateTime.Now
                        };

                        db.PointDetails.Add(pointDetail);
                        await db.SaveChangesAsync(); // 保存點數明細

                        // 2. 更新 Members 資料表中的 Points 欄位
                        var member = await db.Members.FindAsync(pointDetailRequest.MemberId);
                        if (member == null)
                        {
                            return NotFound(); // 如果找不到對應會員
                        }

                        member.Points = pointDetailRequest.Remaining; // 更新會員的最新點數
                        db.Entry(member).State = System.Data.Entity.EntityState.Modified;
                        await db.SaveChangesAsync(); // 保存變更

                        // 提交所有變更
                        transaction.Commit();

                        return Ok(new { message = "點數明細和會員點數更新成功" });
                    }
                    catch (Exception ex)
                    {
                        // 如果出錯則回滾交易
                        transaction.Rollback();
                        return BadRequest("更新點數時出錯");
                    }
                }
            }
        }
    }
}
