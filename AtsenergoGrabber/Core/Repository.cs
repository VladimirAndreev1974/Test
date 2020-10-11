using AtsenergoGrabber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;

namespace AtsenergoGrabber.Core
{
    public class Repository
    {
        private ReportContext context;

        public Repository(ReportContext context)
        {
            this.context = context;

        }


        public async Task<IEnumerable<LossModel>> GetByDate(string date)
        {

            return await context.Losses.Where(l => l.ReportDate == date)
                .Select(l => new LossModel { Region = l.Region, Amount = l.Amount }).ToListAsync();

        }


        public async Task Add(List<LossModel> data, string reportDate)
        {

            var oldData = await context.Losses.Where(l => l.ReportDate == reportDate).ToArrayAsync();

            if (oldData.Count() > 0)
            {
                context.Losses.RemoveRange(oldData);
            }

            foreach (var model in data)
            {

                context.Losses.Add(new Loss { Region = model.Region, Amount = model.Amount, ReportDate = reportDate });
            }

            await context.SaveChangesAsync();

        }

    }
}