//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading;
//using uGIF;
//using UnityEngine;
//
//class BitmapEncoder
//{
//    public static void WriteBitmap(Stream stream, int width, int height, byte[] imageData)
//    {
//        using (BinaryWriter bw = new BinaryWriter(stream))
//        {
//            // define the bitmap file header
//            bw.Write((UInt16) 0x4D42); // bfType;
//            bw.Write((UInt32) (14 + 40 + (width * height * 4))); // bfSize;
//            bw.Write((UInt16) 0); // bfReserved1;
//            bw.Write((UInt16) 0); // bfReserved2;
//            bw.Write((UInt32) 14 + 40); // bfOffBits;
//
//            // define the bitmap information header
//            bw.Write((UInt32) 40); // biSize;
//            bw.Write((Int32) width); // biWidth;
//            bw.Write((Int32) height); // biHeight;
//            bw.Write((UInt16) 1); // biPlanes;
//            bw.Write((UInt16) 32); // biBitCount;
//            bw.Write((UInt32) 0); // biCompression;
//            bw.Write((UInt32) (width * height * 4)); // biSizeImage;
//            bw.Write((Int32) 0); // biXPelsPerMeter;
//            bw.Write((Int32) 0); // biYPelsPerMeter;
//            bw.Write((UInt32) 0); // biClrUsed;
//            bw.Write((UInt32) 0); // biClrImportant;
//
//            // switch the image data from RGB to BGR
//            for (int imageIdx = 0; imageIdx < imageData.Length; imageIdx += 3)
//            {
//                bw.Write(imageData[imageIdx + 2]);
//                bw.Write(imageData[imageIdx + 1]);
//                bw.Write(imageData[imageIdx + 0]);
//                bw.Write((byte) 255);
//            }
//        }
//    }
//}
//
//[RequireComponent(typeof(Camera))]
//public class LiveRecord : MonoBehaviour
//{
//    public int downscale = 1;
//    public bool useBiLinearScaling = true;
//
//    public int imageCount;
//    public float scaleRatio;
//
//    private Queue<Texture2D> _images;
//    private Queue<Texture2D> _tempImages;
//    private Camera _cam;
//
//    private int _width;
//    private int _height;
//
//    private RenderTexture _renderTexture;
//
//    private Queue<Texture2D> _texturePool;
//
//    private Rect _rect;
//
//    private bool _recording;
//
//    [NonSerialized] private byte[] _bytes;
//    private readonly List<Image> _frames = new List<Image>();
//
//    private void Awake()
//    {
//        _cam = GetComponent<Camera>();
//        _images = new Queue<Texture2D>(imageCount);
//        _tempImages = new Queue<Texture2D>(imageCount);
//
//        _width = (int) (Screen.width * scaleRatio);
//        _height = (int) (Screen.height * scaleRatio);
//
//        _renderTexture = new RenderTexture(_width, _height, 24);
//
//        _texturePool = new Queue<Texture2D>(imageCount);
//
//        _rect = new Rect(0, 0, _width, _height);
//
//        for (int i = 0; i < imageCount; i++)
//        {
//            var temp = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
//            _texturePool.Enqueue(temp);
//        }
//    }
//
//    private void OnGUI()
//    {
//        if (GUI.Button(new Rect(10, 70, 200, 100), "Record Bug"))
//        {
//            RecordMovie();
//        }
//
//        if (GUI.Button(new Rect(10, 270, 200, 100), "Start/Stop Recording"))
//        {
//            _recording = !_recording;
//        }
//
//        GUI.Label(new Rect(10, 470, 200, 100), $"recording: {_recording}");
//    }
//
//    private void LateUpdate()
//    {
//        if (!_recording) return;
//
//        RecordFrame();
//    }
//
//    private void RecordFrame()
//    {
//        if (_images.Count >= imageCount)
//        {
//            var oldTexture = _images.Dequeue();
//            _texturePool.Enqueue(oldTexture);
//        }
//
//        _cam.targetTexture = _renderTexture;
//        var screenShot = _texturePool.Dequeue();
//        _cam.Render();
//        RenderTexture.active = _renderTexture;
//        screenShot.ReadPixels(_rect, 0, 0);
//        _cam.targetTexture = null;
//        RenderTexture.active = null;
//
//        _images.Enqueue(screenShot);
//    }
//
//    private void Encode(string filePath)
//    {
//        _bytes = null;
////        var thread = new Thread(_Encode);
////        thread.Start();
//        _Encode();
//        StartCoroutine(WaitForBytes(filePath));
//    }
//
//    private IEnumerator WaitForBytes(string filePath)
//    {
//        while (_bytes == null) yield return null;
//        File.WriteAllBytes($"{filePath}/_gameplay_recording_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.gif", _bytes);
//        _bytes = null;
//
//        Debug.Log("Recording Done!");
//    }
//
//    private void _Encode()
//    {
//        var ge = new GIFEncoder
//        {
//            useGlobalColorTable = true,
//            repeat = 0,
//            FPS = 60,
//            transparent = new Color32(255, 0, 255, 255),
//            dispose = 1
//        };
//
//        var stream = new MemoryStream();
//        ge.Start(stream);
//
//        foreach (var f in _frames)
//        {
//            if (downscale != 1)
//            {
//                if (useBiLinearScaling)
//                {
//                    f.ResizeBilinear(f.width / downscale, f.height / downscale);
//                }
//                else
//                {
//                    f.Resize(downscale);
//                }
//            }
//
//            f.Flip();
//            ge.AddFrame(f);
//        }
//
//        ge.Finish();
//        _bytes = stream.GetBuffer();
//        stream.Close();
//    }
//
//    private void RecordMovie()
//    {
//        Debug.Log("Recording...");
//
//        var filePath = $"{Application.persistentDataPath}/gameplay_capture";
//        if (!Directory.Exists(filePath))
//        {
//            Directory.CreateDirectory(filePath);
//        }
//
//        _frames.Clear();
//        _tempImages = new Queue<Texture2D>(_images);
//        while (_tempImages.Count > 0)
//        {
//            var image = _tempImages.Dequeue();
//            _frames.Add(new Image(image));
//        }
//
//        Encode(filePath);
//    }
//
//    private void EncodeAndSave()
//    {
//        print("SCREENRECORDER IO THREAD STARTED");
//
//        var frameQueue = new Queue<byte[]>();
//
//        _tempImages = new Queue<Texture2D>(_images);
//        while (_tempImages.Count > 0)
//        {
//            var image = _tempImages.Dequeue();
//            frameQueue.Enqueue(image.GetRawTextureData());
//        }
//
//        var frameCount = 0;
//        while (true)
//        {
//            if (frameQueue.Count > 0)
//            {
//                // Generate file path
//                string path = Application.persistentDataPath + "/frame" + frameCount + ".bmp";
//
//                // Dequeue the frame, encode it as a bitmap, and write it to the file
//                using (FileStream fileStream = new FileStream(path, FileMode.Create))
//                {
//                    BitmapEncoder.WriteBitmap(fileStream, _width, _height, frameQueue.Dequeue());
//                    fileStream.Close();
//                }
//
//                frameCount++;
//            }
//            else
//            {
//                break;
//            }
//        }
//    }
//
//    public class ScreenRecorder : MonoBehaviour
//    {
//        // Public Properties
//        public int maxFrames; // maximum number of frames you want to record in one video
//        public int frameRate = 30; // number of frames to capture per second
//
//        // The Encoder Thread
//        private Thread encoderThread;
//
//        // Texture Readback Objects
//        private RenderTexture tempRenderTexture;
//        private Texture2D tempTexture2D;
//
//        // Timing Data
//        private float captureFrameTime;
//        private float lastFrameTime;
//        private int frameNumber;
//        private int savingFrameNumber;
//
//        // Encoder Thread Shared Resources
//        private Queue<byte[]> frameQueue;
//        private string persistentDataPath;
//        private int screenWidth;
//        private int screenHeight;
//        private bool threadIsProcessing;
//        private bool terminateThreadWhenDone;
//
//        void Start()
//        {
//            // Set target frame rate (optional)
//            Application.targetFrameRate = frameRate;
//
//            // Prepare the data directory
//            persistentDataPath = Application.persistentDataPath + "/ScreenRecorder";
//
//            print("Capturing to: " + persistentDataPath + "/");
//
//            if (!System.IO.Directory.Exists(persistentDataPath))
//            {
//                System.IO.Directory.CreateDirectory(persistentDataPath);
//            }
//
//            // Prepare textures and initial values
//            screenWidth = GetComponent<Camera>().pixelWidth;
//            screenHeight = GetComponent<Camera>().pixelHeight;
//
//            tempRenderTexture = new RenderTexture(screenWidth, screenHeight, 0);
//            tempTexture2D = new Texture2D(screenWidth, screenHeight, TextureFormat.RGB24, false);
//            frameQueue = new Queue<byte[]>();
//
//            frameNumber = 0;
//            savingFrameNumber = 0;
//
//            captureFrameTime = 1.0f / (float) frameRate;
//            lastFrameTime = Time.time;
//
//            // Kill the encoder thread if running from a previous execution
//            if (encoderThread != null && (threadIsProcessing || encoderThread.IsAlive))
//            {
//                threadIsProcessing = false;
//                encoderThread.Join();
//            }
//
//            // Start a new encoder thread
//            threadIsProcessing = true;
//            encoderThread = new Thread(EncodeAndSave);
//            encoderThread.Start();
//        }
//
//        void OnDisable()
//        {
//            // Reset target frame rate
//            Application.targetFrameRate = -1;
//
//            // Inform thread to terminate when finished processing frames
//            terminateThreadWhenDone = true;
//        }
//
//        void OnRenderImage(RenderTexture source, RenderTexture destination)
//        {
//            if (frameNumber <= maxFrames)
//            {
//                // Check if render target size has changed, if so, terminate
//                if (source.width != screenWidth || source.height != screenHeight)
//                {
//                    threadIsProcessing = false;
//                    this.enabled = false;
//                    throw new UnityException("ScreenRecorder render target size has changed!");
//                }
//
//                // Calculate number of video frames to produce from this game frame
//                // Generate 'padding' frames if desired framerate is higher than actual framerate
//                float thisFrameTime = Time.time;
//                int framesToCapture = ((int) (thisFrameTime / captureFrameTime)) -
//                                      ((int) (lastFrameTime / captureFrameTime));
//
//                // Capture the frame
//                if (framesToCapture > 0)
//                {
//                    Graphics.Blit(source, tempRenderTexture);
//
//                    RenderTexture.active = tempRenderTexture;
//                    tempTexture2D.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
//                    RenderTexture.active = null;
//                }
//
//                // Add the required number of copies to the queue
//                for (int i = 0; i < framesToCapture && frameNumber <= maxFrames; ++i)
//                {
//                    frameQueue.Enqueue(tempTexture2D.GetRawTextureData());
//
//                    frameNumber++;
//
//                    if (frameNumber % frameRate == 0)
//                    {
//                        print("Frame " + frameNumber);
//                    }
//                }
//
//                lastFrameTime = thisFrameTime;
//            }
//            else //keep making screenshots until it reaches the max frame amount
//            {
//                // Inform thread to terminate when finished processing frames
//                terminateThreadWhenDone = true;
//
//                // Disable script
//                this.enabled = false;
//            }
//
//            // Passthrough
//            Graphics.Blit(source, destination);
//        }
//
//        private void EncodeAndSave()
//        {
//            print("SCREENRECORDER IO THREAD STARTED");
//
//            while (threadIsProcessing)
//            {
//                if (frameQueue.Count > 0)
//                {
//                    // Generate file path
//                    string path = persistentDataPath + "/frame" + savingFrameNumber + ".bmp";
//
//                    // Dequeue the frame, encode it as a bitmap, and write it to the file
//                    using (FileStream fileStream = new FileStream(path, FileMode.Create))
//                    {
//                        BitmapEncoder.WriteBitmap(fileStream, screenWidth, screenHeight, frameQueue.Dequeue());
//                        fileStream.Close();
//                    }
//
//                    // Done
//                    savingFrameNumber++;
//                    print("Saved " + savingFrameNumber + " frames. " + frameQueue.Count + " frames remaining.");
//                }
//                else
//                {
//                    if (terminateThreadWhenDone)
//                    {
//                        break;
//                    }
//
//                    Thread.Sleep(1);
//                }
//            }
//
//            terminateThreadWhenDone = false;
//            threadIsProcessing = false;
//
//            print("SCREENRECORDER IO THREAD FINISHED");
//        }
//    }
//}