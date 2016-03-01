using ProjectS3.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
namespace tuvantuyensinhsv.v2.Controllers
{
    public class SystemInfo
    {
        ProjectS3Entities db = new ProjectS3Entities();
        public async Task AddValue(string key, int value)
        {
            SystemInformation info = db.SystemInformation.SingleOrDefault(t => t.Key == key);

            if (info == null)
            {
                info = new SystemInformation();
                info.Key = key;
                info.value = value;
                db.SystemInformation.Add(info);
                await db.SaveChangesAsync();               
            }
            else
            {
                info.value += value;
                db.Entry(info).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }
        public void SetValue(string key, int value)
        {
            SystemInformation info = db.SystemInformation.SingleOrDefault(t => t.Key == key);

            if (info == null)
            {
                info = new SystemInformation();
                info.Key = key;
                info.value = value;
                db.SystemInformation.Add(info);
                db.SaveChanges();
            }
            else
            {
                info.value = value;
                db.Entry(info).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void RemoveValue(string key)
        {
            SystemInformation info = db.SystemInformation.SingleOrDefault(t => t.Key == key);

            if (info == null)
            {
                return;
            }
            else
            {
                db.SystemInformation.Remove(info);
                db.SaveChanges();
            }
        }  
    }
}