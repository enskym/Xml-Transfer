using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Xml.Integration.Data.Helper
{
    public class FileIoHelper : IDisposable
    {
        public static void SyncFile(HttpPostedFileBase httpPostedFile, string fileName, string directory, string fileHeadName)
        {
            if (httpPostedFile == null)
                return;
            if (httpPostedFile.ContentLength <= 0)
                return;
            var path = HttpContext.Current.Server.MapPath(Path.Combine(directory, fileName));
            try
            {

                RemoveFiles(fileHeadName);
                httpPostedFile.SaveAs(path);

            }
            catch (Exception exception)
            {
                throw new Exception("Add file error.", exception);
            }
        }

        private static void RemoveFiles(string file)
        {
            var fileName = string.Format("{0}.{1}", file, "xls");
            var path1 = GetProjectFilePath(fileName, "//Content//");
            var fileName2 = string.Format("{0}.{1}", file, "xlsx");
            var path2 = GetProjectFilePath(fileName2, "//Content//");
            Remove(path1);
            Remove(path2);
        }
        public static string GetProjectFilePath(string fileName, string directory)
        {
            return HttpContext.Current.Server.MapPath(Path.Combine(directory, fileName));
        }

        public static void Remove(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return;

                File.Delete(path);

            }
            catch (Exception exception)
            {
                throw new Exception("Add file error.", exception);
            }
        }

        public static void AddSyncCustomer(HttpPostedFileBase fileBase)
        {
            try
            {
                var file = "CustomerUpload";
                var fileName = string.Format("{0}{1}", file, Path.GetExtension(fileBase.FileName));
                SyncFile(fileBase, fileName, "//Content//", file);
            }
            catch (Exception)
            {
                // ignored
            }
        }


        public static string GetCommonFileName(string fileHeadName)
        {
            var fileName = string.Format("{0}.{1}", fileHeadName, "xls");
            var path1 = GetProjectFilePath(fileName, "//Content//");
            if (File.Exists(path1))
                return path1;

            var fileName2 = string.Format("{0}.{1}", fileHeadName, "xlsx");
            var path2 = GetProjectFilePath(fileName2, "//Content//");
            return File.Exists(path2) ? path2 : null;
        }

        public static string GetCommonFileNamePrim(string fileHeadName)
        {
            var fileName = string.Format("{0}.{1}", fileHeadName, "xls");
            var path1 = GetProjectFilePath(fileName, "//Prim//");
            if (File.Exists(path1))
                return path1;

            var fileName2 = string.Format("{0}.{1}", fileHeadName, "xlsx");
            var path2 = GetProjectFilePath(fileName2, "//Prim//");
            return File.Exists(path2) ? path2 : null;
        }

        public static DataTable ReadSycnedExcel(bool hasHeader, string fileHeadName)
        {
            var path = GetCommonFileName(fileHeadName);

            if (path == null) return null;

            using (var pck = new ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                var tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text.Trim() : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    var row = tbl.Rows.Add();
                    foreach (var cell in wsRow.ToList())
                    {
                        row[cell.Start.Column - 1] = cell.Text.Trim();
                    }
                }

                return tbl;
            }
        }

        public static DataTable ReadSycnedExcelPrim(bool hasHeader, string fileHeadName)

        {
            var path = GetCommonFileNamePrim(fileHeadName);


            if (path == null) return null;

            using (var pck = new ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }

                var ws = pck.Workbook.Worksheets.First();
                var sheetName = ws.Name;

                var tbl = new DataTable();

                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    if (hasHeader)
                    {
                        var hg = firstRowCell.Text?.Trim();
                        if (hg != null)
                        {
                            if (tbl.Columns.Contains(hg))
                            {
                                if (tbl.Columns.Contains(hg + "."))
                                {
                                    tbl.Columns.Add(hg + "..");
                                }
                                else
                                {
                                    tbl.Columns.Add(hg + ".");
                                }
                            }
                            else
                            {
                                tbl.Columns.Add(hg);
                            }
                        }
                        else
                        {
                            tbl.Columns.Add("");

                        }
                    }
                    else
                    {
                        tbl.Columns.Add(string.Format("", firstRowCell?.Start?.Column));
                    }

                }

                var startRow = hasHeader ? 2 : 1;
                for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    var row = tbl.Rows.Add();
                    foreach (var cell in wsRow.ToList())
                    {
                        try
                        {
                            row[cell.Start.Column - 1] = cell.Text?.Trim();
                        }
                        catch (Exception e)
                        {
                            //row[cell.Start.Column - 1] = "";
                        }
                    }
                }

                return tbl;
            }
        }


        public static string XlsxToHTML(byte[] file)
        {
            MemoryStream stream = new MemoryStream(file);
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("<table>");

            using (ExcelPackage excelPackage = new ExcelPackage(stream))
            {
                ExcelWorkbook workbook = excelPackage.Workbook;
                if (workbook != null)
                {
                    ExcelWorksheet worksheet = workbook.Worksheets.FirstOrDefault();
                    if (worksheet != null)
                    {
                        var firstCell = worksheet.Cells[1, 1].Value;
                        var secondCell = worksheet.Cells[1, 2].Value;

                        stringBuilder.Append("<tr><td>" + firstCell + "</td></tr>");
                        stringBuilder.Append("<tr><td>" + firstCell + "</td></tr>");
                    }
                }
            }

            stringBuilder.Append("</table>");

            return stringBuilder.ToString();
        }

        //public static List<FicheListDto> ReadFiche()
        //{
        //    var readedList = ReadSycnedExcel(true, "CustomerUpload").AsEnumerable().ToList();
        //    try
        //    {
        //        //farklı bir model kullan 
        //        var list = readedList.Select(row => new FicheListDto
        //        {
        //            Year = row.Field<string>("Year"),
        //            Month = row.Field<string>("Month"),
        //            Date = row.Field<string>("Date"),
        //            AccountName = row.Field<string>("AccountName"),
        //            AccountCode = row.Field<string>("AccountCode"),
        //            Debit = row.Field<string>("Debit"),
        //            Balance = row.Field<string>("Balance"),
        //            Credit = row.Field<string>("Credit"),
        //            OzelKod2 = row.Field<string>("OzelKod2"),
        //            Departman = row.Field<string>("Departman"),
        //            CostCenterCodes = row.Field<string>("CostCenterCodes"),
        //            CostCenterExplanation = row.Field<string>("CostCenterExplanation"),
        //            Description = row.Field<string>("Description"),
        //            GrupAdi = row.Field<string>("GrupAdi"),
        //        }).ToList();

        //        return list;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //public static List<TargetDto> ReadTarget()
        //{
        //    var readedList = ReadSycnedExcel(true, "TargetUpload").AsEnumerable().ToList();
        //    try
        //    {
        //        //farklı bir model kullan 
        //        var list = readedList.Select(row => new TargetDto
        //        {
        //            Year = row.Field<string>("Year"),
        //            Month = row.Field<string>("Month"),
        //            Departman = row.Field<string>("Departman"),
        //            SpeCode2 = row.Field<string>("OzelKod2"),
        //            Value = row.Field<string>("Hedef"),
        //            CostCenterExplan = row.Field<string>("CostCenterExplan"),
        //            CostCenterCodes = row.Field<string>("CostCenterCodes"),
        //            AccountName = row.Field<string>("AccountName"),
        //            GrupAdi = row.Field<string>("GrupAdi")
        //        }).ToList();

        //        return list;


        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public static void AddSyncDealer(HttpPostedFileBase fileBase)
        {

            try
            {
                var file = "DealerUserUpload";
                var fileName = string.Format("{0}{1}", file, Path.GetExtension(fileBase.FileName));
                SyncFile(fileBase, fileName, "//Content//", file);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        //public static List<DealerUserExcelListDto> ReadDealer()
        //{
        //    try
        //    {
        //        var readedList = ReadSycnedExcel(true, "DealerUserUpload").AsEnumerable().ToList();

        //        //farklı bir model kullan 
        //        var list = readedList.Select(row => new DealerUserExcelListDto
        //        {
        //            BK = row.Field<string>("BK"),
        //            Bayi = row.Field<string>("Bayi"),
        //            SD = row.Field<string>("SD"),
        //            Destek = row.Field<string>("Destek"),
        //            ST = row.Field<string>("ST"),
        //            Temsilci = row.Field<string>("Temsilci"),
        //            SY = row.Field<string>("SY"),
        //            Yonetici = row.Field<string>("Yonetici"),
        //        }).ToList();

        //        return list;

        //    }
        //    catch (Exception e)
        //    {
        //        return new List<DealerUserExcelListDto>();
        //    }
        //}

        public static void AddSync(HttpPostedFileBase fileBase, string random)
        {
            try
            {
                var file = random;
                var fileName = string.Format("{0}{1}", file, Path.GetExtension(fileBase.FileName));
                SyncFile(fileBase, fileName, "//Content//", file);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public static DataTable Read(string random)
        {
            try
            {
                var readedList = ReadSycnedExcel(true, random);
                return readedList;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static DataTable ReadPrim(string random)
        {
            try
            {
                var readedList = ReadSycnedExcelPrim(true, random);
                return readedList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
