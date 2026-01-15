// 1. Kết nối SignalR
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var currentConvId = 0;

// Khi nhận tin nhắn mới
connection.on("ReceiveMessage", function (senderId, message, time) {
    if (currentConvId > 0) { // Nếu đang mở khung chat
        appendMessage(senderId, message, time);
        scrollToBottom();
    } else {
        // Có thể thêm code rung chuông thông báo ở đây nếu chat đang đóng
    }
});

connection.start().catch(err => console.error(err));

// 2. Các hàm xử lý giao diện
function toggleChatBox() {
    var box = document.getElementById('liveChatBox');
    var btn = document.getElementById('chatFloatingBtn');

    if (box.style.display === 'none') {
        if (currentConvId === 0) {
            alert("Bạn chưa chọn cuộc trò chuyện nào! Hãy vào danh sách ứng viên hoặc khóa học để bắt đầu chat.");
            return;
        }
        box.style.display = 'flex';
        btn.style.display = 'none';
        scrollToBottom();
    } else {
        box.style.display = 'none';
        btn.style.display = 'flex';
    }
}

// Hàm này được gọi từ các nút "Chat ngay" ở trang Tuyển dụng/Học viên
function startChat(targetUserId, courseId, titleName) {
    // Gọi API để lấy ID hội thoại
    $.post('/Chat/OpenChat', { targetUserId: targetUserId, courseId: courseId, type: 'Recruitment' }, function (res) {
        if (res.success) {
            currentConvId = res.conversationId;
            document.getElementById('chatTitle').innerText = titleName;

            // Join group SignalR
            connection.invoke("JoinConversation", currentConvId.toString());

            // Load lịch sử cũ
            $.get('/Chat/GetHistory?conversationId=' + currentConvId, function (msgs) {
                $('#chatHistory').html(''); // Clear cũ
                msgs.forEach(m => appendMessage(m.senderId, m.content, m.time));

                // Mở Chat Box lên
                document.getElementById('liveChatBox').style.display = 'flex';
                document.getElementById('chatFloatingBtn').style.display = 'none';
                scrollToBottom();
            });
        }
    });
}

function sendChatMessage() {
    var msg = document.getElementById('msgInput').value;
    if (!msg.trim()) return;

    // Gửi lên Server
    connection.invoke("SendMessage", parseInt(currentConvId), parseInt(myUserId), msg);
    document.getElementById('msgInput').value = '';
}

function appendMessage(senderId, message, time) {
    var isMine = (senderId == myUserId);
    var div = document.createElement('div');
    div.className = isMine ? 'msg-row msg-mine' : 'msg-row msg-other';
    div.innerHTML = `<div class="msg-bubble">${message}</div><span class="msg-time">${time}</span>`;
    document.getElementById('chatHistory').appendChild(div);
}

function scrollToBottom() {
    var div = document.getElementById('chatHistory');
    div.scrollTop = div.scrollHeight;
}

// Enter để gửi
document.getElementById('msgInput').addEventListener('keypress', function (e) {
    if (e.key === 'Enter') sendChatMessage();
});