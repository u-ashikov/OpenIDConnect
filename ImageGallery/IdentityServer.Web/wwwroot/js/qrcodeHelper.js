window.addEventListener("load", function () {
    const keyUri = document.getElementById("qrCodeKeyUri");
    new QRCode(document.getElementById("qrCode"),
        {
            text: keyUri.innerText,
            width: 150,
            height: 150,
            correctLevel : QRCode.CorrectLevel.H
        });
});