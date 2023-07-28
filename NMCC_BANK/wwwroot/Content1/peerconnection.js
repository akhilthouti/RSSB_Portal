'use strict';

//const startButton = document.getElementById(meetingOptions.startButton);
const callButton = document.getElementById(meetingOptions.callButton);
const answerButton = document.getElementById(meetingOptions.answerButton);
const hangupButton = document.getElementById(meetingOptions.hangupButton);
const snapshotButton = document.getElementById(meetingOptions.btnSnapshot);
const msgWindow = document.getElementById(meetingOptions.msgWindow);
const sendMsg = document.getElementById(meetingOptions.sendMsg);
const msgTxt = document.getElementById(meetingOptions.msgTxt);
const recordbutton = document.getElementById(meetingOptions.record);

const myNo = Math.floor((Math.random() * 100) + 1);
var isRemoteSet = false;
var iceCandidates = [];
var remoteIceCandidates = [];
//var iceServers = [{
//    urls: 'turn:corp.hirdhav.com:3478',
//    username: 'hirdhav',
//    credential: 'hirdhav'
//}];
var iceServers = [];

callButton.disabled = true;
hangupButton.disabled = true;
//recordbutton.disabled = true;
//startButton.addEventListener('click', start);
callButton.addEventListener('click', call);
answerButton.addEventListener('click', answer);
hangupButton.addEventListener('click', hangup);
snapshotButton.addEventListener('click', Takesnapshot);

sendMsg.addEventListener('click', sendMessage);

let startTime;
const localVideo = document.getElementById(meetingOptions.localVideo);
const remoteVideo = document.getElementById(meetingOptions.remoteVideo);

localVideo.addEventListener('loadedmetadata', function () {
    console.log(`Local video videoWidth: ${this.videoWidth}px,  videoHeight: ${this.videoHeight}px`);
});

remoteVideo.addEventListener('loadedmetadata', function () {
    console.log(`Remote video videoWidth: ${this.videoWidth}px,  videoHeight: ${this.videoHeight}px`);
});

remoteVideo.addEventListener('resize', () => {
    console.log(`Remote video size changed to ${remoteVideo.videoWidth}x${remoteVideo.videoHeight}`);
    // We'll use the first onsize callback as an indication that video has started
    // playing out.
    if (startTime) {
        const elapsedTime = window.performance.now() - startTime;
        console.log('Setup time: ' + elapsedTime.toFixed(3) + 'ms');
        startTime = null;
    }
});

let localStream;
let remoteStream;
let pc1;
let pc2;
var webRTC;

function getName(pc) {
    return (pc === pc1) ? 'pc1' : 'pc2';
}

function getOtherPc(pc) {
    return (pc === pc1) ? pc2 : pc1;
}

async function start() {
    console.log('Requesting local stream');
    //startButton.disabled = true;
    try {
        const stream = await navigator.mediaDevices.getUserMedia({
            audio: true, video: {
                width: { ideal: 1280 }
            }
        });
        console.log('Received local stream');
        localVideo.srcObject = stream;
        localVideo.autoPlay = true;
        localStream = stream;
        callButton.disabled = false;
    } catch (e) {
        alert(`getUserMedia() error: ${e.name}`);
    }

    webRTC = new WebRTC();
    //webRTC.connectToSocket('wss://localhost/VKYCWebAPI/ws');
    webRTC.connectToSocket('wss://api.indofinnet.com/ws');
}

async function call() {
    callButton.disabled = true;
    hangupButton.disabled = false;
    
    console.log('Starting call');
    startTime = window.performance.now();
    const videoTracks = localStream.getVideoTracks();
    const audioTracks = localStream.getAudioTracks();
    if (videoTracks.length > 0) {
        console.log(`Using video device: ${videoTracks[0].label}`);
    }
    if (audioTracks.length > 0) {
        console.log(`Using audio device: ${audioTracks[0].label}`);
    }
    const configuration = {
        iceServers: iceServers
    };
    console.log('RTCPeerConnection configuration:', configuration);
    pc1 = new RTCPeerConnection(configuration);
    console.log('Created local peer connection object pc1');

    //let camTrack = (await navigator.mediaDevices.getUserMedia({ video: true })).getTracks()[0];
    //let camTransceiver = pc1.addTransceiver(camTrack, { direction: "sendrecv" });
    localStream.getTracks().forEach((track) => { pc1.addTrack(track, localStream); });
    pc1.addTransceiver("video");
    console.log('Added local stream to pc1');

    pc1.addEventListener('icecandidate', e => onIceCandidate(pc1, e));
    pc1.addEventListener('iceconnectionstatechange', e => onIceStateChange(pc1, e));
    pc1.addEventListener('track', e => gotRemoteStream(e));

    try {
        console.log('pc1 createOffer start');
        const offer = await pc1.createOffer();
        await onCreateOfferSuccess(offer);
    } catch (e) {
        onCreateSessionDescriptionError(e);
    }
}

async function answer() {
    const videoTracks = localStream.getVideoTracks();
    const audioTracks = localStream.getAudioTracks();
    if (videoTracks.length > 0) {
        console.log(`Using video device: ${videoTracks[0].label}`);
    }
    if (audioTracks.length > 0) {
        console.log(`Using audio device: ${audioTracks[0].label}`);
    }

    const configuration = {
        iceServers: iceServers
    };
    pc1 = new RTCPeerConnection(configuration);

    localStream.getTracks().forEach(track => pc1.addTrack(track, localStream));
    pc1.addTransceiver("video");
    console.log('Added local stream to pc1');

    console.log('Created remote peer connection object pc1');
    pc1.addEventListener('icecandidate', e => onIceCandidate(pc1, e));
    pc1.addEventListener('iceconnectionstatechange', e => onIceStateChange(pc1, e));
    pc1.addEventListener('track', e => gotRemoteStream(e));

    //try {
    //    console.log('pc1 createOffer start');
    //    const offer = await pc1.createOffer();
    //    await onCreateOfferSuccess(offer);
    //} catch (e) {
    //    onCreateSessionDescriptionError(e);
    //}

    //let camTrack = (await navigator.mediaDevices.getUserMedia({ video: true })).getTracks()[0];
    //let camTransceiver = pc1.addTransceiver(camTrack, { direction: "sendrecv" });

    //await pc2.setRemoteDescription(remoteDesc);

    var desc = new RTCSessionDescription(user1SDP);

    await pc1.setRemoteDescription(desc);
    onSetRemoteSuccess(pc1);

    const answer = await pc1.createAnswer();
    onCreateAnswerSuccess(answer);

    window.setTimeout(function () { sendIceCandidates(); }, 3000);
}

function onCreateSessionDescriptionError(error) {
    console.log(`Failed to create session description: ${error.toString()}`);
}

async function onCreateOfferSuccess(desc) {
    //console.log(`Offer from pc1\n${desc.sdp}`);
    //console.log('offer form pc1 serial:\n' + JSON.stringify(desc));
    console.log('pc1 setLocalDescription start');

    try {
        //desc.sdp = desc.sdp + "\n";
        await pc1.setLocalDescription(desc);
        onSetLocalSuccess(pc1);
    } catch (e) {
        onSetSessionDescriptionError();
    }
}

function sendMessage() {
    webRTC.send({ From: myNo, To: "", SignalType: 'text', Raw: $(msgTxt).val()});
}

function onSetLocalSuccess(pc) {
    sendSDPToServer(pc1.localDescription);
    console.log(`${getName(pc)} setLocalDescription complete`);
}

function sendSDPToServer(sdp) {
    $.ajax({
        url: baseurl + "/api/Meeting/" + meetingOptions.meetingId + "/sdp",
        type: "POST",
        data: JSON.stringify({ user1SDP: JSON.stringify(sdp) }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function () {
          }
    })

    webRTC.send({ From: myNo, To: "", SignalType: 's2dp', Raw: sdp });
}

function sendSDP2ToServer(sdp) {
    $.ajax({
        url: baseurl + "/api/Meeting/" + meetingOptions.meetingId + "/sdp2",
        type: "POST",
        data: JSON.stringify({ user1SDP: JSON.stringify(sdp) }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function () {
        }
    })

    webRTC.send({ From: myNo, To: "", SignalType: 's2dp', Raw: sdp });
}

function onSetRemoteSuccess(pc) {
    isRemoteSet = true;
    console.log(`${getName(pc)} setRemoteDescription complete`);
    sendIceCandidates();
    //addIceCandidates();
}

function onSetSessionDescriptionError(error) {
    console.log(`Failed to set session description: ${error.toString()}`);
}

function gotRemoteStream(e) {
    //pc1.getReceivers().forEach(function (receiver) {
    ////    receiver.getAudioTracks().forEach(function (audio) {
    ////        remoteStream.addTrack(audio);
    ////    });
    ////    receiver.getVideoTracks().forEach(function (video) {
    ////        remoteStream.addTrack(video);
    ////    });
    //    remoteStream.addTrack(receiver.track);
    //});
    remoteStream = new MediaStream();
    pc1.getRemoteStreams()[0].getTracks().forEach(function (track) {
        console.log("Adding remote stream: ");
        console.log(track);
        remoteStream.addTrack(track);
    })
    remoteVideo.srcObject = remoteStream;
    remoteVideo.autoPlay = true;
    remoteVideo.playsInline = true;
    remoteVideo.muted = false;

    snapshotButton.disabled = false;
    hangupButton.disabled = false;
    recordbutton.disabled = false;
}

async function onCreateAnswerSuccess(desc) {
    //console.log(`Answer from pc1:\n${desc.sdp}`);
    //console.log('Answer from pc1 answer : ' + JSON.stringify(desc));
    console.log('pc1 setLocalDescription start');
    try {
        await pc1.setLocalDescription(desc);
        onSetLocalSuccess(pc1);
    } catch (e) {
        onSetSessionDescriptionError(e);
    }
    /*console.log('pc2 setRemoteDescription start');
    try {
        await pc2.setRemoteDescription(desc);
        onSetRemoteSuccess(pc2);
    } catch (e) {
        onSetSessionDescriptionError(e);
    }*/
}

async function onIceCandidate(pc, event) {
    try {
        if (event.candidate) {
            iceCandidates.push(event.candidate);
        }
        //webRTC.send({ From: myNo, To: "", SignalType: "iceCandidates", Raw: event.candidate });
        //if (isRemoteSet) {
        //    await (pc1.addIceCandidate(event.candidate));
        //    onAddIceCandidateSuccess(pc1);
        //}
        //else {
        //    iceCandidates.push(event.candidate);
        //}
    } catch (e) {
        onAddIceCandidateError(pc, e);
    }
    //console.log(`${getName(pc)} ICE candidate:\n${event.candidate ? event.candidate.candidate : '(null)'}`);
}

function sendIceCandidates() {
    //console.log("ice to send:" + JSON.stringify(iceCandidates));
    iceCandidates.map((ice) => {
        webRTC.send({ From: myNo, To: "", SignalType: "iceCandidates", Raw: ice });
    })
    //$.ajax({
    //    url: baseurl + "/api/Meeting/" + meetingOptions.meetingId + "/icecandidate",
    //    type: "POST",
    //    data: JSON.stringify({
    //        id: meetingOptions.meetingId, iceCandidates: JSON.stringify(iceCandidates)
    //    }),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function () {
    //        console.log("ice candidates sent: " + JSON.stringify(iceCandidates));
    //    }
    //})

    //$.post(baseurl + "/api/Meeting/" + meetingOptions.meetingId + "/icecandidate", new { iceCandidates }).done(function () {
    //    console.log("ice candidates sent: " + JSON.stringify(iceCandidates));
    //})
}

function recIceCandidates() {
    $.ajax({
        url: baseurl + "/api/Meeting/" + meetingOptions.meetingId + "/icecandidate",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            console.log("got ice candidates: " + result);
            remoteIceCandidates = JSON.parse(result);
            addIceCandidates();
        }
    })
}

function addIceCandidates() {
    /*var ice = remoteIceCandidates.pop();
    if (ice != undefined) {
        pc1.addIceCandidate(ice);
        onAddIceCandidateSuccess(pc1);
        addIceCandidates();
    } else {
        addIceCandidates();
    }*/
    remoteIceCandidates.map((ice) => {
        pc1.addIceCandidate(ice);
        onAddIceCandidateSuccess(pc1);
    })
}

function onAddIceCandidateSuccess(pc) {
    console.log(`${getName(pc)} addIceCandidate success`);
}

function onAddIceCandidateError(pc, error) {
    console.log(`${getName(pc)} failed to add ICE Candidate: ${error.toString()}`);
}

function onIceStateChange(pc, event) {
    if (pc) {
        console.log(`${getName(pc)} ICE state: ${pc.iceConnectionState}`);
        console.log('ICE state change event: ', event);
    }
}

function hangup() {
    pc1.close();
    pc1 = null;

    localVideo.srcObject = null;

    hangupButton.disabled = true;
    callButton.disabled = false;
    snapshotButton.disabled = true;
    window.location.href = '/CPO/EndCP';
 
}

function Takesnapshot() {
    snapshotcanvas.width = remoteVideo.videoWidth;
    snapshotcanvas.height = remoteVideo.videoHeight;
    snapshotcanvas.getContext('2d').drawImage(remoteVideo, 0, 0, snapshotcanvas.width, snapshotcanvas.height);
}

$(document).ready(function () {
    let xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            let res = JSON.parse(xhr.responseText);
            console.log("response: ", res);
            //alert(JSON.stringify(res.v.iceServers));
            iceServers.push(res.v.iceServers);
        }
    }
    xhr.open("PUT", "https://global.xirsys.net/_turn/VKYC", true);
    xhr.setRequestHeader("Authorization", "Basic " + btoa("dhavalhirdhav:a484848a-30a4-11ec-bdca-0242ac150003"));
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.send(JSON.stringify({ "format": "urls" }));

    //$("#acceptAnswer").click(function () {
    //    $.get(baseurl + "/api/Meeting/" + meetingOptions.meetingId).done(function (result) {
    //        if (result != null) {
    //            user2SDP = JSON.parse(result.user1SDP);
    //            console.log("got user2sdp");

    //            var desc = new RTCSessionDescription(user2SDP);
    //            pc1.setRemoteDescription(desc);

    //            onSetRemoteSuccess(pc1);
    //        }
    //    });
    //});
})