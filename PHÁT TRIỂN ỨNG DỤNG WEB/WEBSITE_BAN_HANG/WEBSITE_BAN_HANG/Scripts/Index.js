$(document).ready(function () {
    function InDonHang(content) {
        var printWindow = window.open('', '', 'height=400,width=800');
        printWindow.document.write('<html><head><title>DIV Contents</title>');
        printWindow.document.write('</head><body >');
        printWindow.document.write(content);
        printWindow.document.write('</body></html>');
        printWindow.document.close();
        printWindow.print();
    }

    $("h1").click(function () {
        var content = $(".hoadon").html();
        InDonHang(content);
    });
   
});
