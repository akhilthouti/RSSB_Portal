'use strict';

/* globals main */

// This code is adapted from
// https://rawgit.com/Miguelao/demos/master/mediarecorder.html

/* globals main, MediaRecorder */

var mediaSource = new MediaSource();
//mediaSource.addEventListener('sourceopen', handleSourceOpen, false);
var mediaRecorder;
var mediaRecorder2;
var recordedBlobs;
var recordedBlobs2;
var sourceBuffer;
var stream;
var stream2;

const canvas = document.getElementById(meetingOptions.canvas);
const video = document.getElementById(meetingOptions.localVideo);
const video2 = document.getElementById(meetingOptions.remoteVideo);
const recorded = document.getElementById(meetingOptions.recorded);
//const recorded2 = document.getElementById(meetingOptions.recorded2);

$(document).ready(function () {
    
    $("#" + meetingOptions.record).click(toggleRecording);
    $("#" + meetingOptions.play).click(play);
})

function InitializeCanvas() {
    canvas.width = 1280; //video.videoWidth;
    canvas.height = 720; //video.videoHeight;
    canvas.getContext('2d').drawImage(video, 0, 200, 639, video2.videoHeight);
    canvas.getContext('2d').drawImage(video2, 640, 200, 640, video2.videoHeight);

    stream = localStream;
    stream2 = remoteStream;
    //stream = canvas.captureStream(); // frames per second
}
//const recordButton = document.querySelector('button#record');
const playButton = document.getElementById(meetingOptions.play);
const downloadButton = document.getElementById(meetingOptions.download);

//recordButton.onclick = toggleRecording;
//playButton.onclick = play;
downloadButton.onclick = download;

// Start the GL teapot on the canvas
//main();


console.log('Started stream capture from canvas element: ', stream);

function handleSourceOpen(event) {
    console.log('MediaSource opened');
    sourceBuffer = mediaSource.addSourceBuffer('video/webm; codecs="vp8"');
    console.log('Source buffer: ', sourceBuffer);
}

function handleDataAvailable(event) {
    if (event.data && event.data.size > 0) {
        recordedBlobs.push(event.data);
    }
}

function handleDataAvailable2(event) {
    if (event.data && event.data.size > 0) {
        recordedBlobs2.push(event.data);
    }
}

function handleStop(event) {
    console.log('Recorder stopped: ', event);
    const superBuffer = new Blob(recordedBlobs, { type: 'video/webm' });
    video.src = window.URL.createObjectURL(superBuffer);
}

function handleStop2(event) {
    console.log('Recorder stopped: ', event);
    const superBuffer = new Blob(recordedBlobs2, { type: 'video/webm' });
    video.src = window.URL.createObjectURL(superBuffer);
}

function toggleRecording() {
    if ($("#" + meetingOptions.record).text() === 'Start Recording') {
        startRecording();
    } else if ($("#" + meetingOptions.record).text() === 'Stop Recording') {
        stopRecording();
        //stopRecording2();
        $("#" + meetingOptions.record).html('Start Recording');
        playButton.disabled = false;
        downloadButton.disabled = false;

    }
}

// The nested try blocks will be simplified when Chrome 47 moves to Stable
function startRecording() {
    InitializeCanvas();
    var options = { mimeType: 'video/webm' };
    recordedBlobs = [];
    recordedBlobs2 = [];
    try {
        var videoStream = canvas.captureStream(30);

        let ctx = new AudioContext();
        let dest = ctx.createMediaStreamDestination();
        let sourceNode = ctx.createMediaStreamSource(localStream);
        let remoteNode = ctx.createMediaStreamSource(remoteStream);
        sourceNode.connect(dest);
        remoteNode.connect(dest);

        let remoteAudioTracks = dest.stream.getAudioTracks();
        remoteAudioTracks.map((audio) => {
            videoStream.addTrack(audio);
        })

        mediaRecorder = new MediaRecorder(videoStream);
        //mediaRecorder = new MediaRecorder(stream, options);
        //mediaRecorder2 = new MediaRecorder(stream2, options);
    } catch (e0) {
        console.log('Unable to create MediaRecorder with options Object: ', e0);
        try {
            options = { mimeType: 'video/webm,codecs=vp9' };
            mediaRecorder = new MediaRecorder(videoStream);
            //mediaRecorder = new MediaRecorder(stream, options);
            //mediaRecorder2 = new MediaRecorder(stream2, options);
        } catch (e1) {
            console.log('Unable to create MediaRecorder with options Object: ', e1);
            try {
                options = 'video/vp8'; // Chrome 47
                mediaRecorder = new MediaRecorder(videoStream);
                //mediaRecorder = new MediaRecorder(stream, options);
                //mediaRecorder2 = new MediaRecorder(stream2, options);
            } catch (e2) {
                alert('MediaRecorder is not supported by this browser.\n\n' +
                    'Try Firefox 29 or later, or Chrome 47 or later, ' +
                    'with Enable experimental Web Platform features enabled from chrome://flags.');
                console.error('Exception while creating MediaRecorder:', e2);
                return;
            }
        }
    }
    //mediaRecorder.setAudioSource(0);
    //mediaRecorder.setOutputFormat(MediaRecorder.OutputFormat.MPEG_4);
    //mediaRecorder.setAudioEncoder(MediaRecorder.AudioEncoder.AMR_NB);
    //mediaRecorder.prepare();
    console.log('Created MediaRecorder', mediaRecorder, 'with options', options);
    //console.log('Created MediaRecorder2', mediaRecorder2, 'with options', options);
    $("#" + meetingOptions.record).html('Stop Recording');
    playButton.disabled = true;
    downloadButton.disabled = true;
    //recordbutton.disabled = true;
    //mediaRecorder.onstart = startRecording;
    mediaRecorder.onstop = handleStop;
    mediaRecorder.ondataavailable = handleDataAvailable;
    mediaRecorder.start(); // collect 100ms of data

    //mediaRecorder2.onstop = handleStop2;
    //mediaRecorder2.ondataavailable = handleDataAvailable2;
    //mediaRecorder2.start();
    //mediaRecorder.onstart = handlestart;
    //console.log('MediaRecorder started', mediaRecorder);
    window.requestAnimationFrame(loop);
}

function loop() {
    //canvas.getContext('2d').drawImage(video, 0, 0, video.videoWidth, video.videoHeight);
    //canvas2.getContext('2d').drawImage(video2, 0, 0, video2.videoWidth, video2.videoHeight);

    canvas.getContext('2d').drawImage(video, 0, 200, 639, video2.videoHeight);
    canvas.getContext('2d').drawImage(video2, 640, 200, 640, video2.videoHeight);

    window.requestAnimationFrame(loop);
}

function stopRecording() {
    mediaRecorder.stop();

    setTimeout(function () {
        uploadVideo();
    }, 1000);

    console.log('Recorded Blobs: ', recordedBlobs);
    //video.controls = true;
}

function stopRecording2() {
    mediaRecorder2.stop();
    console.log('Recorded Blobs: ', recordedBlobs2);
    //video.controls = true;
}

function play() {

    //$("#" + meetingOptions.mainwin).hide();
    const blob = new Blob(recordedBlobs, { type: 'video/webm' });


    recorded.src = window.URL.createObjectURL(blob);
    recorded.play();
    
}

function play2() {
    //const blob = new Blob(recordedBlobs2, { type: 'video/webm' });
    //recorded2.src = window.URL.createObjectURL(blob);
    //recorded2.play();
}

function uploadVideo() {
    const blob = new Blob(recordedBlobs, { type: 'video/webm' });
    var fd = new FormData();
    fd.append('data', blob);

    $.ajax({
        url: baseurl + "/api/meeting/" + meetingOptions.meetingId + "/upload",
        type: 'POST',
        data: fd,
        processData: false,
        contentType: false
    }).done(function (data) {
    });
}

function download() {
    const blob = new Blob(recordedBlobs, { type: 'video/webm' });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.style.display = 'none';
    a.href = url;
    a.download = 'test.webm';
    document.body.appendChild(a);
    a.click();
    setTimeout(() => {
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
    }, 100);
}