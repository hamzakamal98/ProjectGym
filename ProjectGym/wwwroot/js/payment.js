
function CreatePaymentsDb()
{
    debugger

    var CreatePay = {
        Amount: $('#Ammount').val(),
        PaymentDate: $('#PaymentDate').val(),
        PaymentType: $('#ddlpaymentMethod').val(),
        SubscriptionId : $('#SubscriptionId').val()
    }

    $.ajax({
        type: "POST",
        url: '/Payments/CreatePayments',
        contentType: "application/json",
        data: JSON.stringify(CreatePay),
       
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    title: "Success!",
                    text: response.message,
                    icon: "success",
                    confirmButtonText: "OK"
                }).then(() => {
                    window.location.href = "/Home/"; 
                });
            } else {
                Swal.fire({
                    title: "Error",
                    text: "Something went wrong. Please try again.",
                    icon: "error",
                    confirmButtonText: "OK"
                });
            }
        },
        error: function (xhr, status, error) {
            console.error("خطأ في الطلب:", error);
            console.log("تفاصيل الخطأ:", xhr.responseText);
        }
    });
}