using BreakfastOrderSystem.Site.Models.Repositories;
using BreakfastOrderSystem.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreakfastOrderSystem.Site.Models.Services
{
    public class PointService
    {
        private readonly PointRepository _pointRepository;

        //public PointService()
        //{
        //}

        public PointService()
        {
            _pointRepository = new PointRepository();
            
        }

        
        public IEnumerable<PointDetailVm> GetPointDetail()
        {
            var pointDetails = _pointRepository.GetPointDetail();
            var pointDetailVm = pointDetails.Select(x => new PointDetailVm
            {
                Id = x.Id,
                MemberId = x.MemberId,
                MemberName = x.Member.Name,
                OrderId = x.OrderId,
                Original = x.Original,
                Used = x.Used,
                Earned = x.Earned,
                Date = x.Date,
                Remaining = x.Remaining,

            }).ToList();

            return pointDetailVm;

        }
    }
}