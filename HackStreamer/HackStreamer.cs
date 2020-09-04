using System.Runtime.InteropServices;
using WindowsMediaLib;
using DShowNET.Device;
using DShowNET;
using System;
using System.Collections;

namespace Hack
{
    public class HackStreamer
    {
        //Local var$
        //private IFilterGraph2 m_FilterGraph = null;
        private DsDevice audioDevChosen = null;
        private DsDevice videoDevChosen = null;
        private IGraphBuilder m_FilterGraph = null;  //?
        private IMediaControl m_mediaCtrl = null;
        private IWMWriterAdvanced2 m_writerAdvanced2 = null;
        private string outputFilename = ".\\captured.wmv";
        private string customGoodProfile = "ICAgICAgICA8cHJvZmlsZSB2ZXJzaW9uPSI1ODk4MjQiICAgICAgICAgICAgICAgc3RvcmFnZWZvcm1hdD0iMSIgICAgICAgICAgICAgICBuYW1lPSJIaWdoLXF1YWxpdHkgVkJSIGZvciBQQUwiICAgICAgICAgICAgICAgZGVzY3JpcHRpb249IkEgcHJvZmlsZSBmb3IgY3JlYXRpbmcgaGlnaC1xdWFsaXR5IFZCUiBjb250ZW50IGZyb20gUEFMIHJlY29yZGluZ3MuIj4gICAgICAgICAgICAgICAgICAgICA8c3RyZWFtY29uZmlnIG1ham9ydHlwZT0iezczNjQ3NTYxLTAwMDAtMDAxMC04MDAwLTAwQUEwMDM4OUI3MX0iICAgICAgICAgICAgICAgICAgICAgc3RyZWFtbnVtYmVyPSIxIiAgICAgICAgICAgICAgICAgICAgIHN0cmVhbW5hbWU9IkF1ZGlvIFN0cmVhbSIgICAgICAgICAgICAgICAgICAgICBpbnB1dG5hbWU9IkF1ZGlvNDEzIiAgICAgICAgICAgICAgICAgICAgIGJpdHJhdGU9IjEyODAxNiIgICAgICAgICAgICAgICAgICAgICBidWZmZXJ3aW5kb3c9IjUwMDAiICAgICAgICAgICAgICAgICAgICAgcmVsaWFibGV0cmFuc3BvcnQ9IjAiICAgICAgICAgICAgICAgICAgICAgZGVjb2RlcmNvbXBsZXhpdHk9IiIgICAgICAgICAgICAgICAgICAgICByZmMxNzY2bGFuZ2lkPSJubCIgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHZicmVuYWJsZWQ9IjEiICAgICAgICAgICAgICAgICAgICAgICAgICAgICB2YnJxdWFsaXR5PSI5MCIgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGJpdHJhdGVtYXg9IjAiICAgICAgICAgICAgICAgICAgICAgICAgICAgICBidWZmZXJ3aW5kb3dtYXg9IjAiPiAgICAgICAgICAgICAgIDx3bW1lZGlhdHlwZSBzdWJ0eXBlPSJ7MDAwMDAxNjEtMDAwMC0wMDEwLTgwMDAtMDBBQTAwMzg5QjcxfSIgICAgICAgICAgICAgICAgICAgICAgYmZpeGVkc2l6ZXNhbXBsZXM9IjEiICAgICAgICAgICAgICAgICAgICAgYnRlbXBvcmFsY29tcHJlc3Npb249IjAiICAgICAgICAgICAgICAgICAgICAgbHNhbXBsZXNpemU9Ijg5MTciPiAgICAgICAgICAgICA8d2F2ZWZvcm1hdGV4IHdGb3JtYXRUYWc9IjM1MyIgICAgICAgICAgICAgICAgICAgICAgICAgICBuQ2hhbm5lbHM9IjIiICAgICAgICAgICAgICAgICAgICAgICAgICAgblNhbXBsZXNQZXJTZWM9IjQ0MTAwIiAgICAgICAgICAgICAgICAgICAgICAgICAgIG5BdmdCeXRlc1BlclNlYz0iMjE0NzQ4MzQ4MiIgICAgICAgICAgICAgICAgICAgICAgICAgICBuQmxvY2tBbGlnbj0iODkxNyIgICAgICAgICAgICAgICAgICAgICAgICAgICB3Qml0c1BlclNhbXBsZT0iMTYiICAgICAgICAgICAgICAgICAgICAgICAgICAgY29kZWNkYXRhPSIwMDg4MDAwMDBGMDA1NThCMDAwMCIvPiAgICAgICAgICAgICAgPC93bW1lZGlhdHlwZT4gICAgICAgICAgICAgPC9zdHJlYW1jb25maWc+ICAgICAgICAgICAgICAgICAgICA8c3RyZWFtY29uZmlnIG1ham9ydHlwZT0iezczNjQ2OTc2LTAwMDAtMDAxMC04MDAwLTAwQUEwMDM4OUI3MX0iICAgICAgICAgICAgICAgICAgICAgc3RyZWFtbnVtYmVyPSIyIiAgICAgICAgICAgICAgICAgICAgIHN0cmVhbW5hbWU9IlZpZGVvIFN0cmVhbSIgICAgICAgICAgICAgICAgICAgICBpbnB1dG5hbWU9IlZpZGVvNDEzIiAgICAgICAgICAgICAgICAgICAgIGJpdHJhdGU9IjEwMDAwMCIgICAgICAgICAgICAgICAgICAgICBidWZmZXJ3aW5kb3c9IjUwMDAiICAgICAgICAgICAgICAgICAgICAgcmVsaWFibGV0cmFuc3BvcnQ9IjAiICAgICAgICAgICAgICAgICAgICAgZGVjb2RlcmNvbXBsZXhpdHk9IkFVIiAgICAgICAgICAgICAgICAgICAgIHJmYzE3NjZsYW5naWQ9Im5sIiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgdmJyZW5hYmxlZD0iMSIgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHZicnF1YWxpdHk9IjkwIiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgYml0cmF0ZW1heD0iMCIgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGJ1ZmZlcndpbmRvd21heD0iMCI+ICAgICAgICAgICAgICAgICAgICAgICA8dmlkZW9tZWRpYXByb3BzIG1heGtleWZyYW1lc3BhY2luZz0iNjAwMDAwMDAiICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcXVhbGl0eT0iODAiLz4gICAgICAgICAgICAgICA8d21tZWRpYXR5cGUgc3VidHlwZT0iezMzNTY0RDU3LTAwMDAtMDAxMC04MDAwLTAwQUEwMDM4OUI3MX0iICAgICAgICAgICAgICAgICAgICAgIGJmaXhlZHNpemVzYW1wbGVzPSIwIiAgICAgICAgICAgICAgICAgICAgIGJ0ZW1wb3JhbGNvbXByZXNzaW9uPSIxIiAgICAgICAgICAgICAgICAgICAgIGxzYW1wbGVzaXplPSIwIj4gICAgICAgICA8dmlkZW9pbmZvaGVhZGVyIGR3Yml0cmF0ZT0iMTAwMDAwIiAgICAgICAgICAgICAgICAgICAgICAgICAgZHdiaXRlcnJvcnJhdGU9IjAiICAgICAgICAgICAgICAgICAgICAgICAgICBhdmd0aW1lcGVyZnJhbWU9IjQwMDAwMCI+ICAgICAgICAgIDxyY3NvdXJjZSBsZWZ0PSIwIiAgICAgICAgICAgICAgICAgICAgdG9wPSIwIiAgICAgICAgICAgICAgICAgICAgcmlnaHQ9IjcyMCIgICAgICAgICAgICAgICAgICAgIGJvdHRvbT0iNTc2Ii8+ICAgICAgICAgIDxyY3RhcmdldCBsZWZ0PSIwIiAgICAgICAgICAgICAgICAgICAgdG9wPSIwIiAgICAgICAgICAgICAgICAgICAgcmlnaHQ9IjcyMCIgICAgICAgICAgICAgICAgICAgIGJvdHRvbT0iNTc2Ii8+ICAgICAgICAgICAgICA8Yml0bWFwaW5mb2hlYWRlciBiaXdpZHRoPSI3MjAiICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBiaWhlaWdodD0iNTc2IiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgYmlwbGFuZXM9IjEiICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBiaWJpdGNvdW50PSIyNCIgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGJpY29tcHJlc3Npb249IldNVjMiICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBiaXNpemVpbWFnZT0iMCIgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGJpeHBlbHNwZXJtZXRlcj0iMCIgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGJpeXBlbHNwZXJtZXRlcj0iMCIgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGJpY2xydXNlZD0iMCIgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGJpY2xyaW1wb3J0YW50PSIwIi8+ICAgICAgICAgPC92aWRlb2luZm9oZWFkZXI+ICAgICAgICAgICAgIDwvd21tZWRpYXR5cGU+ICAgICAgICAgICAgIDwvc3RyZWFtY29uZmlnPiAgICAgIDxzdHJlYW1wcmlvcml0aXphdGlvbj4gICAgICAgPHN0cmVhbSBudW1iZXI9IjEiIG1hbmRhdG9yeT0iMCIvPiAgICAgICA8c3RyZWFtIG51bWJlcj0iMiIgbWFuZGF0b3J5PSIwIi8+ICAgICAgPC9zdHJlYW1wcmlvcml0aXphdGlvbj4gICAgIDwvcHJvZmlsZT4gIAA=";
        //Ctors
        // Custom set devices
        public HackStreamer(DsDevice VideoInputDevice, DsDevice AudioInputDevice)
        {
            audioDevChosen = AudioInputDevice;
            videoDevChosen = VideoInputDevice;
        }
        // Devices by default
        public HackStreamer()
        {
            ArrayList audioDevs = GetAudioDevices();
            ArrayList videoDevs = GetVideoDevices();

            
            if (audioDevs.Count > 0) audioDevChosen = (DsDevice)audioDevs[0];
            if (videoDevs.Count > 0) videoDevChosen = (DsDevice)videoDevs[0];
        }
        //----------------<
        //STATIC
        public static ArrayList GetVideoDevices()
        {
            ArrayList videoDevs;
            DsDev.GetDevicesOfCat(FilterCategory.VideoInputDevice, out videoDevs);
            return videoDevs;
        }
        public static ArrayList GetAudioDevices()
        {
            ArrayList audioDevs;
            DsDev.GetDevicesOfCat(FilterCategory.AudioInputDevice, out audioDevs);
            return audioDevs;
        }

        public void Init(Hashtable config = null)
        {
            //m_FilterGraph = (IFilterGraph2)new FilterGraph();
            m_FilterGraph = (IGraphBuilder)Activator.CreateInstance(Type.GetTypeFromCLSID(Clsid.FilterGraph, true));

            // Get the ICaptureGraphBuilder2
            Guid clsid = Clsid.CaptureGraphBuilder2;
            Guid riid = typeof(ICaptureGraphBuilder2).GUID;
            ICaptureGraphBuilder2 capGraph = (ICaptureGraphBuilder2)DsBugWO.CreateDsInstance(ref clsid, ref riid);
            IBaseFilter capVideoFilter = null;
            IBaseFilter capAudioFilter = null;
            IBaseFilter asfWriter = null;
            IServiceProvider serviceProvider = null;
            int hr;
            object iwmWriter2;

            try
            {
                // Start building the graph
                hr = capGraph.SetFiltergraph(m_FilterGraph);
                Marshal.ThrowExceptionForHR(hr);

                // Add the video device to the graph
                if (videoDevChosen != null)
                {
                    capVideoFilter = GetCapFilter(ref videoDevChosen);
                    hr = m_FilterGraph.AddFilter(capVideoFilter, "Video Capture Device");
                    Marshal.ThrowExceptionForHR(hr);
                }

                // Add the audio device to the graph
                if (audioDevChosen != null)
                {
                    capAudioFilter = GetCapFilter(ref audioDevChosen);
                    hr = m_FilterGraph.AddFilter(capAudioFilter, "Audio Capture Device");
                    Marshal.ThrowExceptionForHR(hr);
                }
                // if we need some shitty quality
                if (config.Contains("shitty"))
                {
                    InitAsfWriter(out asfWriter, true);   
                } else
                {
                    InitAsfWriter(out asfWriter);
                }
                

                //GEtting IWMAdvancedWriter2;
                serviceProvider = (IServiceProvider) asfWriter;
                Guid IID_IWMWriterAdvanced2 = new Guid("{962dc1ec-c046-4db8-9cc7-26ceae500817}");
                hr = serviceProvider.QueryService(IID_IWMWriterAdvanced2, IID_IWMWriterAdvanced2, out iwmWriter2);
                Marshal.ThrowExceptionForHR(hr);

                m_writerAdvanced2 = (IWMWriterAdvanced2)iwmWriter2;
                m_writerAdvanced2.SetLiveSource(true);

                if (config.ContainsKey("cap"))
                {
                    outputFilename = config["cap"] as string;
                    Console.WriteLine("[MODE] Capturing to a local file: {0}", outputFilename);
                }
                IFileSinkFilter cap = (IFileSinkFilter)asfWriter;
                cap.SetFileName(outputFilename, null);

                if (!config.ContainsKey("cap"))
                {
                    //deleting useless sink (writer to a file on a disk).
                    IWMWriterSink uselessSink = null;
                    m_writerAdvanced2.GetSink(0, out uselessSink);
                    m_writerAdvanced2.RemoveSink(uselessSink);
                    if (uselessSink != null)
                    {
                        Marshal.ReleaseComObject(uselessSink);
                        uselessSink = null;
                    }
                }

                if (config.Contains("send"))
                {
                    string url = config["send"] as string;
                    Console.WriteLine("[MODE] Streaming to a remote server: {0}", url);
                    WriterNetworkSink sender = new WriterNetworkSink(url);
                    m_writerAdvanced2.AddSink(sender);
                }
                if (config.Contains("share"))
                {
                    int port = (int)config["share"];
                    WriterNetworkSink listener = new WriterNetworkSink(port);
                    Console.WriteLine("[MODE] Started listening on port {0}", port);
                    m_writerAdvanced2.AddSink(listener);
                }
                //Connecting VideoDev to asfWriter
                if (videoDevChosen != null)
                {
                    hr = capGraph.RenderStream(PinCategory.Capture, MediaType.Video, capVideoFilter, null, asfWriter);
                    //hr = capGraph.RenderStream(null, null, capVideoFilter, null, asfWriter);
                    Marshal.ThrowExceptionForHR(hr);
                }
                //Connecting AudioDev to asfWriter
                if (audioDevChosen != null)
                {
                    hr = capGraph.RenderStream(PinCategory.Capture, MediaType.Audio, capAudioFilter, null, asfWriter);
                    //hr = capGraph.RenderStream(null, null, capAudioFilter, null, asfWriter);
                    Marshal.ThrowExceptionForHR(hr);
                }
                m_mediaCtrl = m_FilterGraph as IMediaControl;
                //debug, dumps graph
                //DirectShowLib.Utils.FilterGraphTools.SaveGraphFile(m_FilterGraph, ".\\mygraph.grf");
            }
            finally
            {
                if (capVideoFilter != null)
                {
                    Marshal.ReleaseComObject(capVideoFilter);
                    capVideoFilter = null;
                }
                if (capAudioFilter != null)
                {
                    Marshal.ReleaseComObject(capAudioFilter);
                    capAudioFilter = null;
                }
                if (asfWriter != null)
                {
                    Marshal.ReleaseComObject(asfWriter);
                    asfWriter = null;
                }
                if (capGraph != null)
                {
                    Marshal.ReleaseComObject(capGraph);
                    capGraph = null;
                }
                if (serviceProvider != null)
                {
                    Marshal.ReleaseComObject(serviceProvider);
                    serviceProvider = null;
                }
               
            }
            Console.WriteLine("INIT done");
        }
        private IBaseFilter GetCapFilter (ref DsDevice dev)
        {
            string s;
            dev.Mon.GetDisplayName(null, null, out s);
            return ((IBaseFilter)Marshal.BindToMoniker(s));
        }
        public void Run ()
        {
            int hr = m_mediaCtrl.Run();
            Marshal.ThrowExceptionForHR(hr);
            Console.WriteLine("RUN done");
        }
        public void Stop ()
        {
            m_mediaCtrl.Stop();
        }
        private void InitAsfWriter(out IBaseFilter asfWriter, bool shittyQuality = false)
        {
            int hr;
            asfWriter = (IBaseFilter)new WMAsfWriter();

            hr = m_FilterGraph.AddFilter(asfWriter, "ASF Writer");
            Marshal.ThrowExceptionForHR(hr);

            if (shittyQuality) return;
            // Create an appropriate IWMProfile from the data
            // Open the profile manager
            IWMProfileManager profileManager;
            IWMProfile wmProfile = null;
            WMUtils.WMCreateProfileManager(out profileManager);

            // error message: The profile is invalid (0xC00D0BC6)
            // E.g. no <prx> tags
            string profile = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(customGoodProfile));
            profileManager.LoadProfileByData(profile, out wmProfile);

            if (profileManager != null)
            {
                Marshal.ReleaseComObject(profileManager);
                profileManager = null;
            }

            // Config only if there is a profile retrieved
            // Set the profile on the writer
            IConfigAsfWriter configWriter = (IConfigAsfWriter)asfWriter;

            configWriter.ConfigureFilterUsingProfile(wmProfile);
        }
        ~HackStreamer()
        {
            if (m_FilterGraph != null)
            {
                Marshal.ReleaseComObject(m_FilterGraph);
                m_FilterGraph = null;
            }
            GC.Collect();
        }
    }

}

