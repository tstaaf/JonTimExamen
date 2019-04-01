Webcam.set({
    width: 320,
    height: 240,
    image_format: 'jpeg',
    jpeg_quality: 90
});
Webcam.attach('#camera');

function takePicture() {
    Webcam.snap(function (data_uri) {
        document.getElementById('results').innerHTML =
            '<img class="center-block" src="' +
            data_uri +
            '"/>';

        Webcam.upload(data_uri,
            '/Visitor/Capture'
    )});
}

function removeAndShowButtons() {
    $("#finishButton").show();
    $("#pictureButton").hide();
    $("#camera").hide();
}

function hideAndShow() {
    $("#qr").show().css({ "margin": "auto" , "background-color": "white" , "color": "black" , "text-align": "center" , "border-radius": "25px"});
    $("#backBtn").show().css({ "position": "absolute" , "right": "15px" , "top": "520px"});
    $("#pic").hide();

}

function textAndBack() {
    $("#qr").hide();
    $("#backBtn").hide();
    $("#captureHeroText").show();
    $("#captureHeroH1").fadeIn(1500);
    $("#captureHeroH2").delay(1800).fadeIn(1500);
    setTimeout(function () {
        window.location = "/Home/Index";
    }, 4500)
}

function checkoutButtonClicked() {
    $("#formDiv").hide();
    $("#checkoutHeroText").show();
    $("#checkoutHeroH1").fadeIn(1500);
    setTimeout(function () {
        $("#checkoutForm").submit();
    }, 4500)
}

//function showInfoText() {
//    $("#infoText")
//}