using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Data;
using NPOI.SS.UserModel;
using System.Collections;

namespace QA.Common.Utilities
{
    public interface IExcelUtilities
    {
        static abstract DataTable XlsxToDataTable(Stream str, string sheetName);
        static abstract DataTable XlsToDataTable(Stream str, string sheetName);
        static abstract Stream DataTableToXlsx(DataTable dt);
        static abstract Stream DataTableToXls(DataTable dt);
    }

    public class ExcelUtilities : IExcelUtilities
    {
        public  static DataTable XlsxToDataTable(Stream str, string sheetName)
        {
            XSSFWorkbook hssfworkbook = new XSSFWorkbook();
            ISheet sheet = hssfworkbook.GetSheet(sheetName);

            DataTable dt = new DataTable();
            IRow headerRow = sheet.GetRow(0);
            IEnumerator rows = sheet.GetRowEnumerator();

            int colCount = headerRow.LastCellNum;
            int rowCount = sheet.LastRowNum;

            for(int c = 0; c< colCount; c++)
            {
                var col = headerRow.GetCell(c);
                if(col == null)
                {
                    colCount = c;
                    break;
                }
                dt.Columns.Add(col.ToString());
            }

            while (rows.MoveNext())
            {
                IRow row = (XSSFRow)rows.Current;
                DataRow dr = dt.NewRow();

                for(int i = 0; i < colCount; i++)
                {
                    ICell cell = row.GetCell(i);

                    if(cell != null)
                    {
                        dr[i] = cell.ToString();
                    }
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public static DataTable XlsToDataTable(Stream str, string sheetName)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet = hssfworkbook.GetSheet(sheetName);

            DataTable dt = new DataTable();
            IRow headerRow = sheet.GetRow(0);
            IEnumerator rows = sheet.GetRowEnumerator();

            int colCount = headerRow.LastCellNum;
            int rowCount = sheet.LastRowNum;

            for (int c = 0; c < colCount; c++)
            {
                var col = headerRow.GetCell(c);
                if (col == null)
                {
                    colCount = c;
                    break;
                }
                dt.Columns.Add(col.ToString());
            }

            while (rows.MoveNext())
            {
                IRow row = (HSSFRow)rows.Current;
                DataRow dr = dt.NewRow();

                for (int i = 0; i < colCount; i++)
                {
                    ICell cell = row.GetCell(i);

                    if (cell != null)
                    {
                        dr[i] = cell.ToString();
                    }
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
        
        public static Stream DataTableToXlsx(DataTable dt)
        {
            MemoryStream stream = new MemoryStream();

            IWorkbook workbook = new XSSFWorkbook(stream);
            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);
            foreach(DataColumn column in dt.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);
            }
            int rowIndex = 1;
            foreach(DataRow row in dt.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach(DataColumn column in dt.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }
                rowIndex++;

            }
            workbook.Write(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public static Stream DataTableToXls(DataTable dt)
        {
            MemoryStream stream = new MemoryStream();

            IWorkbook workbook = new HSSFWorkbook(stream);
            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);
            foreach (DataColumn column in dt.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);
            }
            int rowIndex = 1;
            foreach (DataRow row in dt.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dt.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }
                rowIndex++;

            }
            workbook.Write(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
