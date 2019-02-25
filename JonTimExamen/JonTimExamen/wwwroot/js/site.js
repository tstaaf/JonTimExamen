<script language="JavaScript">
 Webcam.set({
      width: 320,
height: 240,
image_format: 'jpeg',
jpeg_quality: 90
});
Webcam.attach('#my_camera');
</script>

    <script language="JavaScript">
        function take_snapshot() {
            Webcam.snap(function (data_uri) {
                document.getElementById('results').innerHTML =
                '<img src="' + data_uri + '"/>';

            Webcam.upload(data_uri,
                '/Camera/Capture',
                function (code, text) {
                    alert('Photo Captured');
                });
                })
            }
    </script> 