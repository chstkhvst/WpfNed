using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNed.DTO;
using WpfNed.EF;
using System.IO;

namespace WpfNed.Model
{
    public class ContractModel
    {
        Model1 db = new Model1();
        public void AddObj(ContractDTO o)
        {
            var newContract = new Contract
            {
                ReservationId = o.ReservationId,
                UserId = o.UserId,
                SignDate = o.SignDate,
                Total = o.Total
            };
            db.Contract.Add(newContract);
            var reservation = db.Reservation.FirstOrDefault(r => r.Id == newContract.ReservationId);
            var resObj = db.Object.FirstOrDefault(u => u.Id == newContract.Reservation.ObjectId);
            if (resObj != null) resObj.StatusId = 3;

            if (reservation != null)
            {
                reservation.ResStatusId = 2;
                db.SaveChanges();
            }
            //Contract lastContract = db.Contract
            //    .OrderByDescending(c => c.Id) 
            //    .FirstOrDefault(); 
            User em = db.User.FirstOrDefault(r => r.Id == newContract.UserId);
            User cl = reservation.User;
            string street = reservation.Object.Street;
            int building = reservation.Object.Building;
            int ? number = reservation.Object.Number;

            string address;
            if (number == null)
            {
                address = $"ул. {street}, д. {building}";
            }
            else
            {
                address = $"ул. {street}, д. {building}, кв. {number}";
            }

            GenerateAndShowPdf(newContract, em, cl, reservation, address);
            db.SaveChanges();
        }
        public void GenerateAndShowPdf(Contract contract, User em, User cl, Reservation r, string address)
        {
            string filePath = Path.Combine(Path.GetTempPath(), $"Contract_{contract.Id}.pdf");

            using (PdfDocument document = new PdfDocument())
            {
                document.Info.Title = "Договор";

                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Verdana", 20);

                gfx.DrawString($"Договор № {contract.Id}", font, XBrushes.Black, new XRect(0, 20, page.Width, page.Height), XStringFormats.TopCenter);

                double yPosition = 100; 

                font = new XFont("Verdana", 12);

                gfx.DrawString($"Номер брони: {contract.ReservationId}", font, XBrushes.Black, new XRect(50, yPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                yPosition += 30;
                gfx.DrawString($"Сотрудник: {em.FullName}", font, XBrushes.Black, new XRect(50, yPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                yPosition += 30;
                gfx.DrawString($"Дата подписания: {contract.SignDate:dd.MM.yyyy}", font, XBrushes.Black, new XRect(50, yPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                yPosition += 30; 
                gfx.DrawString($"Сумма: {contract.Total:C}", font, XBrushes.Black, new XRect(50, yPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                yPosition += 30;
                gfx.DrawString($"Покупатель: {cl.FullName}", font, XBrushes.Black, new XRect(50, yPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                yPosition += 30; 
                gfx.DrawString($"Владелец: {r.Object.Owner.FullName}", font, XBrushes.Black, new XRect(50, yPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                yPosition += 30; 
                gfx.DrawString($"Объект: {address}", font, XBrushes.Black, new XRect(50, yPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                yPosition += 30;
                gfx.DrawString($"Вид сделки: {r.Object.DealType.DealName}", font, XBrushes.Black, new XRect(50, yPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                yPosition += 30;
                gfx.DrawString("Подпись: ____________ /____________", font, XBrushes.Black, new XRect(50, yPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                string stampImagePath = /*@"..\Images\Stamp.png"*/@"C:\Users\hello\Desktop\uchyoba\WpfNed\Images\Stamp.png"; 
                if (File.Exists(stampImagePath))
                {
                    XImage stampImage = XImage.FromFile(stampImagePath);
                    double imageX = 350; 
                    double imageY = yPosition - 30;
                    double imageWidth = 100;  
                    double imageHeight = 100; 
                    gfx.DrawImage(stampImage, imageX, imageY, imageWidth, imageHeight);
                }
                document.Save(filePath);
            }
            Process.Start(new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }
        public void DeleteObject(ContractDTO obj)
        {
            var existingObject = db.Contract.Find(obj.Id);
            db.Contract.Remove(existingObject);
            db.SaveChanges();
        }
        public void UpdObj(ContractDTO updatedObject)
        {
            var existingObject = db.Contract.FirstOrDefault(obj => obj.Id == updatedObject.Id);

            if (existingObject != null)
            {
                existingObject.UserId = updatedObject.UserId;
                existingObject.Total = updatedObject.Total;

                db.SaveChanges();
                //db.Owner.Load();
                //db.Object.Load();
            }
        }
    }
}
