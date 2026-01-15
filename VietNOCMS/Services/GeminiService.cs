using Newtonsoft.Json;
using System.Text;

namespace VietNOCMS.Services
{
    public class GeminiService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public GeminiService(IConfiguration configuration, HttpClient httpClient) //Dùng HttpClient để gửi POST request lên Google API
        {
            _apiKey = Environment.GetEnvironmentVariable("MY_API_KEY");
            _httpClient = httpClient;
        }

        // Hàm gọi API gốc (Private để tái sử dụng)
        private async Task<string> CallGeminiApi(string prompt)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";
            var requestBody = new
            {
                contents = new[] { new { parts = new[] { new { text = prompt } } } }
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                // In ra console hoặc log để xem Google báo gì
                Console.WriteLine($"Gemini API Error: {errorBody}");

                throw new Exception($"Lỗi kết nối Gemini API ({response.StatusCode}): {errorBody}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
            if (jsonResponse.candidates == null || jsonResponse.candidates.Count == 0)
            {
                // Trường hợp bị Google chặn vì nội dung nhạy cảm hoặc lỗi khác
                if (jsonResponse.promptFeedback != null)
                {
                    return "AI từ chối trả lời do vi phạm chính sách nội dung (Safety Filter).";
                }
                return "AI không trả về kết quả nào. Vui lòng thử lại.";
            }
            // Trả về nội dung text từ AI
            return jsonResponse.candidates[0].content.parts[0].text;
        }

        // 1. AI Tutor (Chat với bài học)
        public async Task<string> ChatWithLessonAsync(string lessonContent, string question)
        {
            var prompt = $@"
                Bạn là một gia sư AI thân thiện.
                Dưới đây là nội dung bài học:
                ---
                {lessonContent}
                ---
                Hãy trả lời câu hỏi: '{question}'.
                Chỉ trả lời dựa trên nội dung bài học. Nếu không có thông tin, hãy nói khéo léo.";
            return await CallGeminiApi(prompt);
        }

        // 2. Tóm tắt bài học
        public async Task<string> SummarizeLessonAsync(string lessonContent)
        {
            var prompt = $@"Hãy tóm tắt nội dung sau thành 5 gạch đầu dòng chính (dùng thẻ HTML <ul><li>): {lessonContent}";
            return await CallGeminiApi(prompt);
        }

        // 3. Chấm bài đa năng (Toán, Văn, Code...)
        public async Task<string> ReviewAssignmentAsync(string subjectContext, string question, string studentAnswer)
        {
            // Tạo vai trò động cho AI
            string role = "giảng viên";
            if (subjectContext.Contains("Toán") || subjectContext.Contains("Lý") || subjectContext.Contains("Hóa"))
                role = "giáo viên Khoa học tự nhiên. Hãy kiểm tra kỹ từng bước tính toán và công thức.";
            else if (subjectContext.Contains("Văn") || subjectContext.Contains("Sử") || subjectContext.Contains("Triết"))
                role = "giáo viên Khoa học xã hội. Hãy kiểm tra tư duy phản biện, dẫn chứng và văn phong.";
            else
                role = "lập trình viên Senior. Hãy kiểm tra logic code, tối ưu và cú pháp.";

            var prompt = $@"
                Đóng vai là một {role}.
                --- ĐỀ BÀI ---
                {question}
                --- BÀI LÀM ---
                {studentAnswer}
                --- YÊU CẦU ---
                Hãy đưa ra nhận xét chi tiết:
                1. **Đánh giá chung:** (Đúng/Sai/Khá/Tốt).
                2. **Chi tiết:** Chỉ ra điểm đúng và điểm sai (nếu sai, hãy chỉ rõ sai ở đâu).
                3. **Gợi ý:** Cách làm hay hơn hoặc sửa lỗi.
                Định dạng trả về: HTML cơ bản (thẻ b, br, ul, li).";

            return await CallGeminiApi(prompt);
        }
        // Trong GeminiService.cs

        public async Task<string> SummarizeDocumentContentAsync(string documentContent)
        {
            // Kiểm tra độ dài, nếu ngắn quá thì không cần tóm tắt
            if (documentContent.Length < 100)
                return "Nội dung file quá ngắn để tóm tắt.";

            // Cắt bớt nếu quá dài (Gemini Flash nhận 1 triệu token ~ 700k từ, nhưng ta nên giới hạn an toàn)
            if (documentContent.Length > 100000)
                documentContent = documentContent.Substring(0, 100000) + "...(đã cắt bớt)";

            var prompt = $@"
        Bạn là một trợ lý học tập thông minh.
        Nhiệm vụ: Tóm tắt nội dung tài liệu dưới đây để học viên ôn tập nhanh.
        
        --- NỘI DUNG TÀI LIỆU ---
        {documentContent}
        
        --- YÊU CẦU ---
        1. Tóm tắt thành 5-7 gạch đầu dòng (dùng thẻ <ul><li>).
        2. Làm nổi bật các từ khóa quan trọng (dùng thẻ <b>).
        3. Nếu là tài liệu kỹ thuật, hãy giữ nguyên các thuật ngữ chuyên ngành.
        4. Ngôn ngữ: Tiếng Việt.";

            return await CallGeminiApi(prompt);
        }
        ////////////////////////////INSTRUCTOR////////////////////////////////
        ///// Trong GeminiService.cs

        // 2.1. QUIZ GENERATOR (Trả về JSON)
        public async Task<string> GenerateQuizFromTextAsync(string topic, int numberOfQuestions = 5)
        {
            var prompt = $@"
                Đóng vai một chuyên gia giáo dục.
                Dựa vào chủ đề/nội dung: '{topic}'.
                Hãy tạo ra {numberOfQuestions} câu hỏi trắc nghiệm.
                
                Yêu cầu định dạng trả về: JSON thuần túy (Array of Objects), không dùng markdown ```json.
                Cấu trúc mỗi câu:
                [
                    {{
                        ""Question"": ""Nội dung câu hỏi?"",
                        ""A"": ""Lựa chọn A"",
                        ""B"": ""Lựa chọn B"",
                        ""C"": ""Lựa chọn C"",
                        ""D"": ""Lựa chọn D"",
                        ""Correct"": ""A"", // Hoặc B, C, D
                        ""Explanation"": ""Giải thích tại sao đúng""
                    }}
                ]
                Ngôn ngữ: Tiếng Việt.";

            return await CallGeminiApi(prompt);
        }

        // 2.2. SYLLABUS BUILDER (Soạn đề cương)
        public async Task<string> GenerateSyllabusAsync(string courseTopic, string level)
        {
            var prompt = $@"
                Tôi muốn dạy một khóa học về chủ đề: '{courseTopic}'.
                Trình độ: {level}.
                
                Hãy giúp tôi lên khung chương trình (Syllabus) chi tiết gồm 5 chương, mỗi chương khoảng 3 bài học.
                Định dạng trả về: HTML (sử dụng thẻ <h4> cho tên chương, <ul><li> cho danh sách bài học).
                Văn phong: Chuyên nghiệp, hấp dẫn, kích thích người học.";

            return await CallGeminiApi(prompt);
        }

        // 2.3. AUTO GRADING (Chấm bài tự luận)
        public async Task<string> AutoGradeAsync(string question, string barem, string studentAnswer)
        {
            var prompt = $@"
                Đóng vai là giáo viên chấm bài khó tính nhưng công tâm.
                
                --- ĐỀ BÀI ---
                {question}
                
                --- ĐÁP ÁN CHUẨN / BAREM ĐIỂM ---
                {barem}
                
                --- BÀI LÀM CỦA HỌC VIÊN ---
                {studentAnswer}
                
                --- YÊU CẦU ---
                Hãy chấm điểm trên thang 10.
                Trả về định dạng JSON thuần túy (không markdown):
                {{
                    ""Score"": 8.5,
                    ""Comment"": ""Nhận xét chi tiết về ưu/nhược điểm..."",
                    ""Suggestion"": ""Gợi ý cách cải thiện...""
                }}";

            return await CallGeminiApi(prompt);
        }
        // Trong GeminiService.cs

        // 1. HÀM ĐỌC CHỮ TỪ ẢNH (OCR)
        public async Task<string> ReadTextFromImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0) return "Lỗi: Không có ảnh.";

            // Chuyển ảnh sang Base64
            string base64Data;
            using (var ms = new MemoryStream())
            {
                await imageFile.CopyToAsync(ms);
                base64Data = Convert.ToBase64String(ms.ToArray());
            }

            // Prompt đơn giản cho OCR
            var prompt = "Hãy chép lại y nguyên toàn bộ văn bản bạn thấy trong bức ảnh này. Không thêm bình luận, không mô tả ảnh. Chỉ trả về văn bản.";

            var requestBody = new
            {
                contents = new[]
                {
            new
            {
                parts = new object[]
                {
                    new { text = prompt },
                    new {
                        inline_data = new {
                            mime_type = imageFile.ContentType, // VD: image/jpeg, image/png
                            data = base64Data
                        }
                    }
                }
            }
        }
            };

            // Gọi API (Tái sử dụng logic gọi HTTP cũ hoặc viết thẳng ở đây)
            // Lưu ý: Bạn có thể copy logic gọi HttpClient từ hàm CallGeminiApi xuống đây 
            // hoặc sửa hàm CallGeminiApi để nhận object requestBody thay vì string prompt.

            // ĐÂY LÀ ĐOẠN GỌI API (Copy lại để code chạy độc lập cho dễ)
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, jsonContent);

            if (!response.IsSuccessStatusCode) return "Lỗi đọc ảnh từ Google.";

            var responseString = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
            return jsonResponse.candidates[0].content.parts[0].text;
        }

        // 2. HÀM CHẤM CODE (Logic Programming)
        public async Task<string> GradeCodeAsync(string question, string studentCode)
        {
            var prompt = $@"
        Bạn là Senior Developer chấm bài thi lập trình.
        
        --- ĐỀ BÀI ---
        {question}
        
        --- CODE CỦA SINH VIÊN ---
        {studentCode}
        
        --- YÊU CẦU ---
        1. Kiểm tra xem code có chạy đúng logic đề bài không? (Dry Run)
        2. Kiểm tra lỗi cú pháp, lỗi bảo mật hoặc hiệu năng.
        3. Chấm điểm (Thang 10).
        
        Trả về JSON: {{ ""Score"": 8, ""Comment"": ""..."" }}";

            return await CallGeminiApi(prompt); // Tái sử dụng hàm CallGeminiApi cũ
        }
        public async Task<string> GenerateAssignmentContentAsync(string title)
        {
            // URL API của Google (sử dụng model gemini-1.5-flash hoặc gemini-pro tùy key của bạn)
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro-latest:generateContent?key={_apiKey}";

            // Câu lệnh (Prompt) gửi cho AI
            var prompt = $@"
        Đóng vai là một giảng viên công nghệ thông tin.
        Dựa vào tiêu đề bài tập: '{title}'.
        Hãy viết một nội dung đề bài chi tiết, bao gồm:
        1. Mục tiêu bài tập.
        2. Các yêu cầu kỹ thuật cụ thể (gạch đầu dòng).
        3. Hướng dẫn/Gợi ý thực hiện.
        
        Yêu cầu định dạng: Viết bằng tiếng Việt, giọng văn chuyên nghiệp. 
        Không dùng Markdown (```html), chỉ trả về text thô có định dạng HTML cơ bản (thẻ p, ul, li, b).";

            var requestBody = new
            {
                contents = new[] { new { parts = new[] { new { text = prompt } } } }
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, jsonContent);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);

                // Trả về kết quả text
                return jsonResponse.candidates[0].content.parts[0].text;
            }
            catch (Exception ex)
            {
                // Trả về thông báo lỗi giả lập nếu gọi API thất bại để không crash app
                return $"Không thể gọi AI lúc này ({ex.Message}). Vui lòng tự nhập nội dung.";
            }
        }

    }
}