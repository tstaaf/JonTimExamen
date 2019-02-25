    Webcam.set({
        width: 320,
        height: 240,
        image_format: 'jpeg',
        jpeg_quality: 90
    });
Webcam.attach('#my_camera');



function take_snapshot() {
    Webcam.snap(function (data_uri) {
    document.getElementById('results').innerHTML =
    '<img src="' + data_uri + '"/>';

    Webcam.upload(data_uri, function (code, text) {
        alert('Photo Captured');
        });
    })
}
