using UglyToad.PdfPig;
using DocumentFormat.OpenXml.Packaging;
using System.Text;

namespace VietNOCMS.Services
{
    public static class DocumentParser
    {
        public static async Task<string> ParseFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return string.Empty;

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0; // Reset con trỏ về đầu file

            var extension = Path.GetExtension(file.FileName).ToLower();

            return extension switch
            {
                ".pdf" => ParsePdf(stream),
                ".docx" => ParseDocx(stream),
                ".doc" => throw new Exception("Vui lòng đổi file .doc sang .docx"),
                ".txt" => ParseTxt(stream),
                _ => throw new Exception("Định dạng file không hỗ trợ!")
            };
        }
        // HÀM MỚI: Hỗ trợ đọc từ đường dẫn file trên server
        public static string ParseLocalFile(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException("Không tìm thấy file tài liệu trên server.");

            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var extension = Path.GetExtension(filePath).ToLower();
            return ParseStream(stream, extension);
        }

        // Logic cốt lõi (Tách riêng để tái sử dụng)
        private static string ParseStream(Stream stream, string extension)
        {
            return extension switch
            {
                ".pdf" => ParsePdf(stream),
                ".docx" => ParseDocx(stream),
                ".txt" => ParseTxt(stream),
                _ => throw new Exception("Định dạng file không hỗ trợ tóm tắt (Chỉ hỗ trợ PDF/DOCX/TXT).")
            };
        }

        // 1. Đọc PDF
        private static string ParsePdf(Stream stream)
        {
            var sb = new StringBuilder();
            try
            {
                using (var document = PdfDocument.Open(stream))
                {
                    foreach (var page in document.GetPages())
                    {
                        sb.AppendLine(page.Text);
                    }
                }
            }
            catch { return "Không thể đọc nội dung PDF (Có thể là file scan/ảnh)."; }
            return sb.ToString();
        }

        // 2. Đọc Word (DOCX)
        private static string ParseDocx(Stream stream)
        {
            var sb = new StringBuilder();
            try
            {
                using (var wordDoc = WordprocessingDocument.Open(stream, false))
                {
                    var body = wordDoc.MainDocumentPart.Document.Body;
                    sb.Append(body.InnerText);
                }
            }
            catch { return "Lỗi đọc file Word."; }
            return sb.ToString();
        }

        // 3. Đọc Text
        private static string ParseTxt(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}