using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;

namespace CPUID
{
    // Token: 0x0200005B RID: 91
    public class CPUIDSDK
    {
        // Token: 0x0600048F RID: 1167 RVA: 0x0002C790 File Offset: 0x0002A990
        public bool IS_F_DEFINED(float _f)
        {
            return _f > 0f;
        }

        // Token: 0x06000490 RID: 1168 RVA: 0x0002C7B0 File Offset: 0x0002A9B0
        public bool IS_F_DEFINED(double _f)
        {
            return _f > 0.0;
        }

        // Token: 0x06000491 RID: 1169 RVA: 0x0002C7D4 File Offset: 0x0002A9D4
        public bool IS_I_DEFINED(int _i)
        {
            return _i != CPUIDSDK.I_UNDEFINED_VALUE;
        }

        // Token: 0x06000492 RID: 1170 RVA: 0x0002C7F4 File Offset: 0x0002A9F4
        public bool IS_I_DEFINED(uint _i)
        {
            return _i != (uint)CPUIDSDK.I_UNDEFINED_VALUE;
        }

        // Token: 0x06000493 RID: 1171 RVA: 0x0002C814 File Offset: 0x0002AA14
        public bool IS_I_DEFINED(short _i)
        {
            return _i != (short)CPUIDSDK.I_UNDEFINED_VALUE;
        }

        // Token: 0x06000494 RID: 1172 RVA: 0x0002C834 File Offset: 0x0002AA34
        public bool IS_I_DEFINED(ushort _i)
        {
            return _i != (ushort)CPUIDSDK.I_UNDEFINED_VALUE;
        }

        // Token: 0x06000495 RID: 1173 RVA: 0x0002C854 File Offset: 0x0002AA54
        public bool IS_I_DEFINED(sbyte _i)
        {
            return _i != (sbyte)CPUIDSDK.I_UNDEFINED_VALUE;
        }

        // Token: 0x06000496 RID: 1174 RVA: 0x0002C874 File Offset: 0x0002AA74
        public bool IS_I_DEFINED(byte _i)
        {
            return _i != (byte)CPUIDSDK.I_UNDEFINED_VALUE;
        }

        // Token: 0x06000497 RID: 1175
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string fileName);

        // Token: 0x06000498 RID: 1176
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr FreeLibrary(IntPtr hModule);

        // Token: 0x06000499 RID: 1177
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        // Token: 0x0600049A RID: 1178 RVA: 0x0002C893 File Offset: 0x0002AA93
        public void InitDLL()
        {
            this._dllHandle = CPUIDSDK.LoadLibrary(".\\cpuidsdk64.dll");
        }

        // Token: 0x0600049C RID: 1180 RVA: 0x0002C9E0 File Offset: 0x0002ABE0
        public void InitSDK()
        {
            this.CPUIDSDK_fp_QueryInterface = (CPUIDSDK.CPUIDSDKfpQueryInterface)Marshal.GetDelegateForFunctionPointer(CPUIDSDK.GetProcAddress(this._dllHandle, "QueryInterface"), typeof(CPUIDSDK.CPUIDSDKfpQueryInterface));
            this.CreateInstance();
            int num = 0;
            int num2 = 0;
            bool flag = this.Init(null, "cpuidsdk.dll", uint.MaxValue, ref num, ref num2);
        }

        // Token: 0x0600049D RID: 1181 RVA: 0x0002CA38 File Offset: 0x0002AC38
        public void InitSDKWithoutHDD()
        {
            this.CPUIDSDK_fp_QueryInterface = (CPUIDSDK.CPUIDSDKfpQueryInterface)Marshal.GetDelegateForFunctionPointer(CPUIDSDK.GetProcAddress(this._dllHandle, "QueryInterface"), typeof(CPUIDSDK.CPUIDSDKfpQueryInterface));
            this.CreateInstance();
            int num = 0;
            int num2 = 0;
            bool flag = this.Init(null, "cpuidsdk.dll", 4294967039u, ref num, ref num2);
        }

        // Token: 0x0600049E RID: 1182 RVA: 0x0002CA94 File Offset: 0x0002AC94
        public void InitSDK_Quick()
        {
            this.CPUIDSDK_fp_QueryInterface = (CPUIDSDK.CPUIDSDKfpQueryInterface)Marshal.GetDelegateForFunctionPointer(CPUIDSDK.GetProcAddress(this._dllHandle, "QueryInterface"), typeof(CPUIDSDK.CPUIDSDKfpQueryInterface));
            this.CreateInstance();
            int num = 0;
            int num2 = 0;
            //bool flag = this.Init(".\\", "cpuidsdk64.dll", 2147483567u, ref num, ref num2);

            // use custom config to accelerate cpuid module startup
            uint config = CPUIDSDK_CONFIG_USE_EVERYTHING;
            config ^= CPUIDSDK_CONFIG_USE_ACPI_TIMER;
            config ^= CPUIDSDK_CONFIG_USE_CHIPSET;
            config ^= CPUIDSDK_CONFIG_USE_DISPLAY_API;
            config ^= CPUIDSDK_CONFIG_USE_DMI;
            config ^= CPUIDSDK_CONFIG_USE_SMBUS;
            config ^= CPUIDSDK_CONFIG_USE_SOFTWARE;
            config ^= CPUIDSDK_CONFIG_USE_SPD;

            this.Init(".\\", "cpuidsdk64.dll", config, ref num, ref num2);
        }

        // Token: 0x0600049F RID: 1183 RVA: 0x0002CAED File Offset: 0x0002ACED
        public void UninitSDK()
        {
            this.Close();
            this.DestroyInstance();
        }

        // Token: 0x060004A0 RID: 1184 RVA: 0x0002CB00 File Offset: 0x0002AD00
        public bool GenerateDump(string _szFilePath)
        {
            bool result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1342653272u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GenerateDump cpuidsdk_fp_GenerateDump = (CPUIDSDK.CPUIDSDK_fp_GenerateDump)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GenerateDump));
                    int num = cpuidsdk_fp_GenerateDump(this.objptr, _szFilePath);
                    bool flag2 = num == 1;
                    if (flag2)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        // Token: 0x060004A1 RID: 1185 RVA: 0x0002CB88 File Offset: 0x0002AD88
        public List<double[]> GetCPUTemperature(Dictionary<uint, List<int>> deviceData, ref string deviceName)
        {
            string text = "";
            float num = 0f;
            float num2 = 0f;
            float num3 = 0f;
            int num4 = 0;
            int num5 = 0;
            num2 = 0f;
            num3 = 0f;
            List<double[]> list = new List<double[]>();
            deviceName = this.GetDeviceName(deviceData[4u][0]);
            int numberOfSensors = this.GetNumberOfSensors(deviceData[4u][0], 8192);
            for (int i = 0; i < numberOfSensors; i++)
            {
                bool sensorInfos = this.GetSensorInfos(deviceData[4u][0], i, 8192, ref num5, ref text, ref num4, ref num, ref num2, ref num3);
                bool flag = sensorInfos && this.IS_F_DEFINED(num);
                if (flag)
                {
                    list.Add(new double[]
                    {
                        (double)num,
                        (double)num2,
                        (double)num3
                    });
                }
            }
            return list;
        }

        // Token: 0x060004A2 RID: 1186 RVA: 0x0002CC74 File Offset: 0x0002AE74
        public List<double[]> GetMainboardTempData(Dictionary<uint, List<int>> deviceData)
        {
            string text = "";
            float num = 0f;
            float num2 = 0f;
            float num3 = 0f;
            int num4 = 0;
            int num5 = 0;
            num2 = 0f;
            num3 = 0f;
            List<double[]> list = new List<double[]>();
            int numberOfSensors = this.GetNumberOfSensors(deviceData[1024u][0], 8192);
            for (int i = 0; i < numberOfSensors; i++)
            {
                bool sensorInfos = this.GetSensorInfos(deviceData[1024u][0], i, 8192, ref num5, ref text, ref num4, ref num, ref num2, ref num3);
                bool flag = sensorInfos && this.IS_F_DEFINED(num);
                if (flag)
                {
                    list.Add(new double[]
                    {
                        (double)num,
                        (double)num2,
                        (double)num3
                    });
                }
            }
            return list;
        }

        // Token: 0x060004A3 RID: 1187 RVA: 0x0002CD50 File Offset: 0x0002AF50
        public Dictionary<string, Dictionary<string, double[]>> GetMainboardFanData()
        {
            Dictionary<string, Dictionary<string, double[]>> dictionary = new Dictionary<string, Dictionary<string, double[]>>();
            string key = "MB";
            string key2 = "";
            float num = 0f;
            float num2 = 0f;
            float num3 = 0f;
            int num4 = 0;
            int num5 = 0;
            num2 = 0f;
            num3 = 0f;
            int numberOfSensors = this.GetNumberOfSensors(0, 12288);
            for (int i = 0; i < numberOfSensors; i++)
            {
                bool sensorInfos = this.GetSensorInfos(0, i, 12288, ref num5, ref key2, ref num4, ref num, ref num2, ref num3);
                bool flag = sensorInfos && Math.Round((double)num, 0) >= 0.0;
                if (flag)
                {
                    try
                    {
                        bool flag2 = dictionary.ContainsKey(key);
                        if (flag2)
                        {
                            dictionary[key].Add(key2, new double[]
                            {
                                Math.Round((double)num, 2),
                                Math.Round((double)num2, 2),
                                Math.Round((double)num3, 2)
                            });
                        }
                        else
                        {
                            dictionary.Add(key, new Dictionary<string, double[]>());
                            bool flag3 = !dictionary[key].ContainsKey(key2);
                            if (flag3)
                            {
                                dictionary[key].Add(key2, new double[]
                                {
                                    Math.Round((double)num, 2),
                                    Math.Round((double)num2, 2),
                                    Math.Round((double)num3, 2)
                                });
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return dictionary;
        }

        // Token: 0x060004A4 RID: 1188 RVA: 0x0002CED0 File Offset: 0x0002B0D0
        public double GetMainboardV3V(Dictionary<uint, List<int>> deviceData)
        {
            int num = 0;
            double num2 = (double)this.GetSensorTypeValue(8392704, ref num, ref num);
            bool flag = this.IS_F_DEFINED(num2);
            double result;
            if (flag)
            {
                result = Math.Round(num2, 2);
            }
            else
            {
                result = -1.0;
            }
            return result;
        }

        // Token: 0x060004A5 RID: 1189 RVA: 0x0002CF20 File Offset: 0x0002B120
        public List<double[]> GetGPUData(Dictionary<uint, List<int>> deviceData, List<string> deviceNames)
        {
            string text = "";
            float num = 0f;
            float num2 = 0f;
            float num3 = 0f;
            int num4 = 0;
            int num5 = 0;
            num2 = 0f;
            num3 = 0f;
            List<double[]> list = new List<double[]>();
            for (int i = 0; i < deviceData[32u].Count; i++)
            {
                int numberOfSensors = this.GetNumberOfSensors(deviceData[32u][i], 8192);
                string deviceName = this.GetDeviceName(deviceData[32u][i]);
                bool flag = deviceName.ToLower().Contains("intel");
                if (!flag)
                {
                    deviceNames.Add(deviceName);
                    for (int j = 0; j < numberOfSensors; j++)
                    {
                        bool sensorInfos = this.GetSensorInfos(deviceData[32u][i], j, 8192, ref num5, ref text, ref num4, ref num, ref num2, ref num3);
                        bool flag2 = sensorInfos && this.IS_F_DEFINED(num);
                        if (flag2)
                        {
                            list.Add(new double[]
                            {
                                (double)num,
                                (double)num2,
                                (double)num3
                            });
                            break;
                        }
                        list.Add(new double[3]);
                    }
                }
            }
            return list;
        }

        // Token: 0x060004A6 RID: 1190 RVA: 0x0002D070 File Offset: 0x0002B270
        public Dictionary<string, Dictionary<string, double[]>> UpdateSensorTemperature()
        {
            Dictionary<string, Dictionary<string, double[]>> dictionary = new Dictionary<string, Dictionary<string, double[]>>();
            string key = "";
            string key2 = "";
            float num = 0f;
            int num2 = 0;
            int num3 = 0;
            float num4 = 0f;
            float num5 = 0f;
            int numberOfDevices = this.GetNumberOfDevices();
            for (int i = 0; i < numberOfDevices; i++)
            {
                key = "#" + i.ToString() + "," + this.GetDeviceName(i);
                int numberOfSensors = this.GetNumberOfSensors(i, 8192);
                for (int j = 0; j < numberOfSensors; j++)
                {
                    bool sensorInfos = this.GetSensorInfos(i, j, 8192, ref num2, ref key2, ref num3, ref num, ref num4, ref num5);
                    bool flag = sensorInfos && this.IS_F_DEFINED(num);
                    if (flag)
                    {
                        try
                        {
                            bool flag2 = dictionary.ContainsKey(key);
                            if (flag2)
                            {
                                dictionary[key].Add(key2, new double[]
                                {
                                    Math.Round((double)num, 2),
                                    Math.Round((double)num4, 2),
                                    Math.Round((double)num5, 2)
                                });
                            }
                            else
                            {
                                dictionary.Add(key, new Dictionary<string, double[]>());
                                bool flag3 = !dictionary[key].ContainsKey(key2);
                                if (flag3)
                                {
                                    dictionary[key].Add(key2, new double[]
                                    {
                                        Math.Round((double)num, 2),
                                        Math.Round((double)num4, 2),
                                        Math.Round((double)num5, 2)
                                    });
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            return dictionary;
        }

        // Token: 0x060004A7 RID: 1191 RVA: 0x0002D218 File Offset: 0x0002B418
        public Dictionary<string, Dictionary<string, double[]>> UpdateSensor()
        {
            Dictionary<string, Dictionary<string, double[]>> dictionary = new Dictionary<string, Dictionary<string, double[]>>();
            string key = "";
            string key2 = "";
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            float num4 = 0f;
            float num5 = 0f;
            float sensorTypeValue = this.GetSensorTypeValue(4198400, ref num2, ref num2);
            bool flag = this.IS_F_DEFINED(sensorTypeValue);
            if (flag)
            {
                dictionary.Add("#-1,VCORE", new Dictionary<string, double[]>
                {
                    {
                        "value",
                        new double[]
                        {
                            Math.Round((double)sensorTypeValue, 2)
                        }
                    }
                });
            }
            sensorTypeValue = this.GetSensorTypeValue(33558528, ref num2, ref num2);
            bool flag2 = this.IS_F_DEFINED(sensorTypeValue);
            if (flag2)
            {
                dictionary.Add("#-1,DRAM_VCORE", new Dictionary<string, double[]>
                {
                    {
                        "value",
                        new double[]
                        {
                            Math.Round((double)sensorTypeValue, 2)
                        }
                    }
                });
            }
            sensorTypeValue = this.GetSensorTypeValue(8392704, ref num2, ref num2);
            bool flag3 = this.IS_F_DEFINED(sensorTypeValue);
            if (flag3)
            {
                dictionary.Add("#-1,V3V", new Dictionary<string, double[]>
                {
                    {
                        "value",
                        new double[]
                        {
                            Math.Round((double)sensorTypeValue, 2)
                        }
                    }
                });
            }
            sensorTypeValue = this.GetSensorTypeValue(12587008, ref num2, ref num2);
            bool flag4 = this.IS_F_DEFINED(sensorTypeValue);
            if (flag4)
            {
                dictionary.Add("#-1,P5V", new Dictionary<string, double[]>
                {
                    {
                        "value",
                        new double[]
                        {
                            Math.Round((double)sensorTypeValue, 2)
                        }
                    }
                });
            }
            sensorTypeValue = this.GetSensorTypeValue(16781312, ref num2, ref num2);
            bool flag5 = this.IS_F_DEFINED(sensorTypeValue);
            if (flag5)
            {
                dictionary.Add("#-1,P12V", new Dictionary<string, double[]>
                {
                    {
                        "value",
                        new double[]
                        {
                            Math.Round((double)sensorTypeValue, 2)
                        }
                    }
                });
            }
            int numberOfDevices = this.GetNumberOfDevices();
            for (int i = 0; i < numberOfDevices; i++)
            {
                key = "#" + i.ToString() + "," + this.GetDeviceName(i);
                bool flag6 = i == 0;
                if (flag6)
                {
                    key = "MB";
                }
                int numberOfSensors = this.GetNumberOfSensors(i, 4096);
                for (int j = 0; j < numberOfSensors; j++)
                {
                    bool sensorInfos = this.GetSensorInfos(i, j, 4096, ref num, ref key2, ref num3, ref sensorTypeValue, ref num4, ref num5);
                    bool flag7 = sensorInfos && sensorTypeValue > -CPUIDSDK.MAX_FLOAT;
                    if (flag7)
                    {
                        try
                        {
                            bool flag8 = dictionary.ContainsKey(key);
                            if (flag8)
                            {
                                dictionary[key][key2] = new double[]
                                {
                                    Math.Round((double)sensorTypeValue, 2),
                                    Math.Round((double)num4, 2),
                                    Math.Round((double)num5, 2)
                                };
                            }
                            else
                            {
                                dictionary.Add(key, new Dictionary<string, double[]>());
                                bool flag9 = !dictionary[key].ContainsKey(key2);
                                if (flag9)
                                {
                                    dictionary[key].Add(key2, new double[]
                                    {
                                        Math.Round((double)sensorTypeValue, 2),
                                        Math.Round((double)num4, 2),
                                        Math.Round((double)num5, 2)
                                    });
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                bool flag10 = i != 0;
                if (flag10)
                {
                    numberOfSensors = this.GetNumberOfSensors(i, 8192);
                    for (int j = 0; j < numberOfSensors; j++)
                    {
                        bool sensorInfos = this.GetSensorInfos(i, j, 8192, ref num, ref key2, ref num3, ref sensorTypeValue, ref num4, ref num5);
                        bool flag11 = sensorInfos && this.IS_F_DEFINED(sensorTypeValue);
                        if (flag11)
                        {
                            try
                            {
                                bool flag12 = dictionary.ContainsKey(key);
                                if (flag12)
                                {
                                    bool flag13 = !dictionary[key].ContainsKey(key2);
                                    if (flag13)
                                    {
                                        dictionary[key].Add(key2, new double[]
                                        {
                                            Math.Round((double)sensorTypeValue, 2),
                                            Math.Round((double)num4, 2),
                                            Math.Round((double)num5, 2)
                                        });
                                    }
                                }
                                else
                                {
                                    dictionary.Add(key, new Dictionary<string, double[]>());
                                    bool flag14 = !dictionary[key].ContainsKey(key2);
                                    if (flag14)
                                    {
                                        dictionary[key].Add(key2, new double[]
                                        {
                                            Math.Round((double)sensorTypeValue, 2),
                                            Math.Round((double)num4, 2),
                                            Math.Round((double)num5, 2)
                                        });
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    numberOfSensors = this.GetNumberOfSensors(i, 12288);
                    for (int j = 0; j < numberOfSensors; j++)
                    {
                        bool sensorInfos = this.GetSensorInfos(i, j, 12288, ref num, ref key2, ref num3, ref sensorTypeValue, ref num4, ref num5);
                        bool flag15 = sensorInfos && Math.Round((double)sensorTypeValue, 0) >= 0.0;
                        if (flag15)
                        {
                            try
                            {
                                bool flag16 = dictionary.ContainsKey(key);
                                if (flag16)
                                {
                                    bool flag17 = !dictionary[key].ContainsKey(key2);
                                    if (flag17)
                                    {
                                        dictionary[key].Add(key2, new double[]
                                        {
                                            Math.Round((double)sensorTypeValue, 2),
                                            Math.Round((double)num4, 2),
                                            Math.Round((double)num5, 2)
                                        });
                                    }
                                }
                                else
                                {
                                    dictionary.Add(key, new Dictionary<string, double[]>());
                                    bool flag18 = !dictionary[key].ContainsKey(key2);
                                    if (flag18)
                                    {
                                        dictionary[key].Add(key2, new double[]
                                        {
                                            Math.Round((double)sensorTypeValue, 2),
                                            Math.Round((double)num4, 2),
                                            Math.Round((double)num5, 2)
                                        });
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    numberOfSensors = this.GetNumberOfSensors(i, 16384);
                    for (int j = 0; j < numberOfSensors; j++)
                    {
                        bool sensorInfos = this.GetSensorInfos(i, j, 16384, ref num, ref key2, ref num3, ref sensorTypeValue, ref num4, ref num5);
                        bool flag19 = sensorInfos && sensorTypeValue > 0f;
                        if (flag19)
                        {
                            try
                            {
                                bool flag20 = dictionary.ContainsKey(key);
                                if (flag20)
                                {
                                    dictionary[key].Add(key2, new double[]
                                    {
                                        Math.Round((double)sensorTypeValue, 2),
                                        Math.Round((double)num4, 2),
                                        Math.Round((double)num5, 2)
                                    });
                                }
                                else
                                {
                                    dictionary.Add(key, new Dictionary<string, double[]>());
                                    bool flag21 = !dictionary[key].ContainsKey(key2);
                                    if (flag21)
                                    {
                                        dictionary[key].Add(key2, new double[]
                                        {
                                            Math.Round((double)sensorTypeValue, 2),
                                            Math.Round((double)num4, 2),
                                            Math.Round((double)num5, 2)
                                        });
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    numberOfSensors = this.GetNumberOfSensors(i, 20480);
                    for (int j = 0; j < numberOfSensors; j++)
                    {
                        bool sensorInfos = this.GetSensorInfos(i, j, 20480, ref num, ref key2, ref num3, ref sensorTypeValue, ref num4, ref num5);
                        bool flag22 = sensorInfos && sensorTypeValue > 0f;
                        if (flag22)
                        {
                            try
                            {
                                bool flag23 = dictionary.ContainsKey(key);
                                if (flag23)
                                {
                                    bool flag24 = !dictionary[key].ContainsKey(key2);
                                    if (flag24)
                                    {
                                        dictionary[key].Add(key2, new double[]
                                        {
                                            Math.Round((double)sensorTypeValue, 2),
                                            Math.Round((double)num4, 2),
                                            Math.Round((double)num5, 2)
                                        });
                                    }
                                }
                                else
                                {
                                    dictionary.Add(key, new Dictionary<string, double[]>());
                                    bool flag25 = !dictionary[key].ContainsKey(key2);
                                    if (flag25)
                                    {
                                        dictionary[key].Add(key2, new double[]
                                        {
                                            Math.Round((double)sensorTypeValue, 2),
                                            Math.Round((double)num4, 2),
                                            Math.Round((double)num5, 2)
                                        });
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    numberOfSensors = this.GetNumberOfSensors(i, 24576);
                    for (int j = 0; j < numberOfSensors; j++)
                    {
                        bool sensorInfos = this.GetSensorInfos(i, j, 24576, ref num, ref key2, ref num3, ref sensorTypeValue, ref num4, ref num5);
                        bool flag26 = sensorInfos;
                        if (flag26)
                        {
                            try
                            {
                                bool flag27 = dictionary.ContainsKey(key);
                                if (flag27)
                                {
                                    dictionary[key].Add(key2, new double[]
                                    {
                                        Math.Round((double)sensorTypeValue, 2),
                                        Math.Round((double)num4, 2),
                                        Math.Round((double)num5, 2)
                                    });
                                }
                                else
                                {
                                    dictionary.Add(key, new Dictionary<string, double[]>());
                                    bool flag28 = !dictionary[key].ContainsKey(key2);
                                    if (flag28)
                                    {
                                        dictionary[key].Add(key2, new double[]
                                        {
                                            Math.Round((double)sensorTypeValue, 2),
                                            Math.Round((double)num4, 2),
                                            Math.Round((double)num5, 2)
                                        });
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    numberOfSensors = this.GetNumberOfSensors(i, 40960);
                    for (int j = 0; j < numberOfSensors; j++)
                    {
                        bool sensorInfos = this.GetSensorInfos(i, j, 40960, ref num, ref key2, ref num3, ref sensorTypeValue, ref num4, ref num5);
                        bool flag29 = sensorInfos && sensorTypeValue > 0f;
                        if (flag29)
                        {
                            try
                            {
                                bool flag30 = dictionary.ContainsKey(key);
                                if (flag30)
                                {
                                    dictionary[key].Add(key2, new double[]
                                    {
                                        Math.Round((double)sensorTypeValue, 2),
                                        Math.Round((double)num4, 2),
                                        Math.Round((double)num5, 2)
                                    });
                                }
                                else
                                {
                                    dictionary.Add(key, new Dictionary<string, double[]>());
                                    bool flag31 = !dictionary[key].ContainsKey(key2);
                                    if (flag31)
                                    {
                                        dictionary[key].Add(key2, new double[]
                                        {
                                            Math.Round((double)sensorTypeValue, 2),
                                            Math.Round((double)num4, 2),
                                            Math.Round((double)num5, 2)
                                        });
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    numberOfSensors = this.GetNumberOfSensors(i, 49152);
                    for (int j = 0; j < numberOfSensors; j++)
                    {
                        bool sensorInfos = this.GetSensorInfos(i, j, 49152, ref num, ref key2, ref num3, ref sensorTypeValue, ref num4, ref num5);
                        bool flag32 = sensorInfos;
                        if (flag32)
                        {
                            try
                            {
                                bool flag33 = dictionary.ContainsKey(key);
                                if (flag33)
                                {
                                    dictionary[key].Add(key2, new double[]
                                    {
                                        Math.Round((double)sensorTypeValue, 2),
                                        Math.Round((double)num4, 2),
                                        Math.Round((double)num5, 2)
                                    });
                                }
                                else
                                {
                                    dictionary.Add(key, new Dictionary<string, double[]>());
                                    bool flag34 = !dictionary[key].ContainsKey(key2);
                                    if (flag34)
                                    {
                                        dictionary[key].Add(key2, new double[]
                                        {
                                            Math.Round((double)sensorTypeValue, 2),
                                            Math.Round((double)num4, 2),
                                            Math.Round((double)num5, 2)
                                        });
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
            }
            return dictionary;
        }

        // Token: 0x060004A8 RID: 1192 RVA: 0x0002DD84 File Offset: 0x0002BF84
        public Dictionary<uint, List<int>> GetInterestDevice()
        {
            int numberOfDevices = this.GetNumberOfDevices();
            Dictionary<uint, List<int>> dictionary = new Dictionary<uint, List<int>>();
            for (int i = 0; i < numberOfDevices; i++)
            {
                bool flag = !dictionary.ContainsKey(4u);
                if (flag)
                {
                    bool flag2 = (long)this.GetDeviceClass(i) == 4L;
                    if (flag2)
                    {
                        dictionary.Add(4u, new List<int>
                        {
                            i
                        });
                    }
                }
                bool flag3 = (long)this.GetDeviceClass(i) == 32L;
                if (flag3)
                {
                    bool flag4 = !dictionary.ContainsKey(32u);
                    if (flag4)
                    {
                        dictionary.Add(32u, new List<int>
                        {
                            i
                        });
                    }
                    else
                    {
                        dictionary[32u].Add(i);
                    }
                }
                bool flag5 = !dictionary.ContainsKey(1024u);
                if (flag5)
                {
                    bool flag6 = (long)this.GetDeviceClass(i) == 1024L;
                    if (flag6)
                    {
                        dictionary.Add(1024u, new List<int>
                        {
                            i
                        });
                    }
                }
            }
            return dictionary;
        }

        // Token: 0x060004A9 RID: 1193 RVA: 0x0002DE8C File Offset: 0x0002C08C
        public bool CreateInstance()
        {
            bool result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3114496838u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_CreateInstance cpuidsdk_fp_CreateInstance = (CPUIDSDK.CPUIDSDK_fp_CreateInstance)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_CreateInstance));
                    this.objptr = cpuidsdk_fp_CreateInstance();
                    bool flag2 = this.objptr != IntPtr.Zero;
                    if (flag2)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    this.objptr = IntPtr.Zero;
                    result = false;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                result = false;
            }
            return result;
        }

        // Token: 0x060004AA RID: 1194 RVA: 0x0002DF30 File Offset: 0x0002C130
        public void DestroyInstance()
        {
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(141955308u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_DestroyInstance cpuidsdk_fp_DestroyInstance = (CPUIDSDK.CPUIDSDK_fp_DestroyInstance)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_DestroyInstance));
                    cpuidsdk_fp_DestroyInstance(this.objptr);
                }
            }
            catch
            {
            }
        }

        // Token: 0x060004AB RID: 1195 RVA: 0x0002DF9C File Offset: 0x0002C19C
        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        public bool Init(string _szDllPath, string _szDllFilename, uint _config_flag, ref int _errorcode, ref int _extended_errorcode)
        {
            bool result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1642906585u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_Init cpuidsdk_fp_Init = (CPUIDSDK.CPUIDSDK_fp_Init)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_Init));
                    int num = cpuidsdk_fp_Init(this.objptr, _szDllPath, _szDllFilename, (int)_config_flag, ref _errorcode, ref _extended_errorcode);
                    bool flag2 = num == 1;
                    if (flag2)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    _errorcode = 16;
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        // Token: 0x060004AC RID: 1196 RVA: 0x0002E044 File Offset: 0x0002C244
        public void Close()
        {
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2482448366u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_Close cpuidsdk_fp_Close = (CPUIDSDK.CPUIDSDK_fp_Close)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_Close));
                    cpuidsdk_fp_Close(this.objptr);
                }
            }
            catch
            {
            }
        }

        // Token: 0x060004AD RID: 1197 RVA: 0x0002E0B0 File Offset: 0x0002C2B0
        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        public void RefreshInformation()
        {
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2716681178u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_RefreshInformation cpuidsdk_fp_RefreshInformation = (CPUIDSDK.CPUIDSDK_fp_RefreshInformation)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_RefreshInformation));
                    cpuidsdk_fp_RefreshInformation(this.objptr);
                }
            }
            catch
            {
            }
        }

        // Token: 0x060004AE RID: 1198 RVA: 0x0002E11C File Offset: 0x0002C31C
        public void GetDllVersion(ref int _version)
        {
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1583791309u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDllVersion cpuidsdk_fp_GetDllVersion = (CPUIDSDK.CPUIDSDK_fp_GetDllVersion)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDllVersion));
                    cpuidsdk_fp_GetDllVersion(this.objptr, ref _version);
                }
            }
            catch
            {
            }
        }

        // Token: 0x060004AF RID: 1199 RVA: 0x0002E18C File Offset: 0x0002C38C
        public int GetNumberOfProcessors()
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1940580181u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetNbProcessors cpuidsdk_fp_GetNbProcessors = (CPUIDSDK.CPUIDSDK_fp_GetNbProcessors)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetNbProcessors));
                    result = cpuidsdk_fp_GetNbProcessors(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004B0 RID: 1200 RVA: 0x0002E204 File Offset: 0x0002C404
        public int GetProcessorFamily(int _proc_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1069580160u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorFamily cpuidsdk_fp_GetProcessorFamily = (CPUIDSDK.CPUIDSDK_fp_GetProcessorFamily)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorFamily));
                    result = cpuidsdk_fp_GetProcessorFamily(this.objptr, _proc_index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004B1 RID: 1201 RVA: 0x0002E280 File Offset: 0x0002C480
        public int GetProcessorCoreCount(int _proc_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2738439794u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorCoreCount cpuidsdk_fp_GetProcessorCoreCount = (CPUIDSDK.CPUIDSDK_fp_GetProcessorCoreCount)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorCoreCount));
                    result = cpuidsdk_fp_GetProcessorCoreCount(this.objptr, _proc_index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004B2 RID: 1202 RVA: 0x0002E2FC File Offset: 0x0002C4FC
        public int GetProcessorThreadCount(int _proc_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3458309187u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorThreadCount cpuidsdk_fp_GetProcessorThreadCount = (CPUIDSDK.CPUIDSDK_fp_GetProcessorThreadCount)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorThreadCount));
                    result = cpuidsdk_fp_GetProcessorThreadCount(this.objptr, _proc_index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004B3 RID: 1203 RVA: 0x0002E378 File Offset: 0x0002C578
        public int GetProcessorCoreThreadCount(int _proc_index, int _core_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(210508056u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorCoreThreadCount cpuidsdk_fp_GetProcessorCoreThreadCount = (CPUIDSDK.CPUIDSDK_fp_GetProcessorCoreThreadCount)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorCoreThreadCount));
                    result = cpuidsdk_fp_GetProcessorCoreThreadCount(this.objptr, _proc_index, _core_index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004B4 RID: 1204 RVA: 0x0002E3F4 File Offset: 0x0002C5F4
        public int GetProcessorThreadAPICID(IntPtr objptr, int _proc_index, int _core_index, int _thread_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(637422588u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorThreadAPICID cpuidsdk_fp_GetProcessorThreadAPICID = (CPUIDSDK.CPUIDSDK_fp_GetProcessorThreadAPICID)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorThreadAPICID));
                    result = cpuidsdk_fp_GetProcessorThreadAPICID(objptr, _proc_index, _core_index, _thread_index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004B5 RID: 1205 RVA: 0x0002E46C File Offset: 0x0002C66C
        public string GetProcessorName(int _proc_index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1068924780u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorName cpuidsdk_fp_GetProcessorName = (CPUIDSDK.CPUIDSDK_fp_GetProcessorName)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorName));
                    IntPtr ptr = cpuidsdk_fp_GetProcessorName(this.objptr, _proc_index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004B6 RID: 1206 RVA: 0x0002E4F4 File Offset: 0x0002C6F4
        public string GetProcessorCodeName(int _proc_index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(235543572u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorCodeName cpuidsdk_fp_GetProcessorCodeName = (CPUIDSDK.CPUIDSDK_fp_GetProcessorCodeName)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorCodeName));
                    IntPtr ptr = cpuidsdk_fp_GetProcessorCodeName(this.objptr, _proc_index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004B7 RID: 1207 RVA: 0x0002E57C File Offset: 0x0002C77C
        public string GetProcessorSpecification(int _proc_index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2313360326u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorSpecification cpuidsdk_fp_GetProcessorSpecification = (CPUIDSDK.CPUIDSDK_fp_GetProcessorSpecification)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorSpecification));
                    IntPtr ptr = cpuidsdk_fp_GetProcessorSpecification(this.objptr, _proc_index);
                    result = Marshal.PtrToStringAnsi(ptr);
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004B8 RID: 1208 RVA: 0x0002E5F8 File Offset: 0x0002C7F8
        public string GetProcessorPackage(int _proc_index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(851731848u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorPackage cpuidsdk_fp_GetProcessorPackage = (CPUIDSDK.CPUIDSDK_fp_GetProcessorPackage)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorPackage));
                    IntPtr ptr = cpuidsdk_fp_GetProcessorPackage(this.objptr, _proc_index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004B9 RID: 1209 RVA: 0x0002E680 File Offset: 0x0002C880
        public string GetProcessorStepping(int _proc_index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1540798381u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorStepping cpuidsdk_fp_GetProcessorStepping = (CPUIDSDK.CPUIDSDK_fp_GetProcessorStepping)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorStepping));
                    IntPtr ptr = cpuidsdk_fp_GetProcessorStepping(this.objptr, _proc_index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004BA RID: 1210 RVA: 0x0002E708 File Offset: 0x0002C908
        public float GetProcessorTDP(int _proc_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2054878453u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorTDP cpuidsdk_fp_GetProcessorTDP = (CPUIDSDK.CPUIDSDK_fp_GetProcessorTDP)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorTDP));
                    result = cpuidsdk_fp_GetProcessorTDP(this.objptr, _proc_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004BB RID: 1211 RVA: 0x0002E784 File Offset: 0x0002C984
        public float GetProcessorManufacturingProcess(int _proc_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3932017851u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorManufacturingProcess cpuidsdk_fp_GetProcessorManufacturingProcess = (CPUIDSDK.CPUIDSDK_fp_GetProcessorManufacturingProcess)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorManufacturingProcess));
                    result = cpuidsdk_fp_GetProcessorManufacturingProcess(this.objptr, _proc_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004BC RID: 1212 RVA: 0x0002E800 File Offset: 0x0002CA00
        public bool IsProcessorInstructionSetAvailable(int _proc_index, int _iset)
        {
            bool result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1468968733u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_IsProcessorInstructionSetAvailable cpuidsdk_fp_IsProcessorInstructionSetAvailable = (CPUIDSDK.CPUIDSDK_fp_IsProcessorInstructionSetAvailable)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_IsProcessorInstructionSetAvailable));
                    int num = cpuidsdk_fp_IsProcessorInstructionSetAvailable(this.objptr, _proc_index, _iset);
                    bool flag2 = num == 1;
                    if (flag2)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        // Token: 0x060004BD RID: 1213 RVA: 0x0002E888 File Offset: 0x0002CA88
        public float GetProcessorCoreClockFrequency(int _proc_index, int _core_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3552159603u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorCoreClockFrequency cpuidsdk_fp_GetProcessorCoreClockFrequency = (CPUIDSDK.CPUIDSDK_fp_GetProcessorCoreClockFrequency)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorCoreClockFrequency));
                    result = cpuidsdk_fp_GetProcessorCoreClockFrequency(this.objptr, _proc_index, _core_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004BE RID: 1214 RVA: 0x0002E904 File Offset: 0x0002CB04
        public float GetProcessorCoreClockMultiplier(int _proc_index, int _core_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3512574651u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorCoreClockMultiplier cpuidsdk_fp_GetProcessorCoreClockMultiplier = (CPUIDSDK.CPUIDSDK_fp_GetProcessorCoreClockMultiplier)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorCoreClockMultiplier));
                    result = cpuidsdk_fp_GetProcessorCoreClockMultiplier(this.objptr, _proc_index, _core_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004BF RID: 1215 RVA: 0x0002E980 File Offset: 0x0002CB80
        public float GetProcessorCoreTemperature(int _proc_index, int _core_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3093000374u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorCoreTemperature cpuidsdk_fp_GetProcessorCoreTemperature = (CPUIDSDK.CPUIDSDK_fp_GetProcessorCoreTemperature)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorCoreTemperature));
                    result = cpuidsdk_fp_GetProcessorCoreTemperature(this.objptr, _proc_index, _core_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004C0 RID: 1216 RVA: 0x0002E9FC File Offset: 0x0002CBFC
        public float GetBusFrequency()
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(152310312u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetBusFrequency cpuidsdk_fp_GetBusFrequency = (CPUIDSDK.CPUIDSDK_fp_GetBusFrequency)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetBusFrequency));
                    result = cpuidsdk_fp_GetBusFrequency(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004C1 RID: 1217 RVA: 0x0002EA78 File Offset: 0x0002CC78
        public float GetProcessorRatedBusFrequency(int _proc_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2019750085u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorRatedBusFrequency cpuidsdk_fp_GetProcessorRatedBusFrequency = (CPUIDSDK.CPUIDSDK_fp_GetProcessorRatedBusFrequency)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorRatedBusFrequency));
                    result = cpuidsdk_fp_GetProcessorRatedBusFrequency(this.objptr, _proc_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004C2 RID: 1218 RVA: 0x0002EAF4 File Offset: 0x0002CCF4
        public float GetProcessorStockClockFrequency(int _proc_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3839609271u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorStockClockFrequency cpuidsdk_fp_GetProcessorStockClockFrequency = (CPUIDSDK.CPUIDSDK_fp_GetProcessorStockClockFrequency)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorStockClockFrequency));
                    result = (float)cpuidsdk_fp_GetProcessorStockClockFrequency(this.objptr, _proc_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004C3 RID: 1219 RVA: 0x0002EB70 File Offset: 0x0002CD70
        public float GetProcessorStockBusFrequency(int _proc_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1909121941u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorStockBusFrequency cpuidsdk_fp_GetProcessorStockBusFrequency = (CPUIDSDK.CPUIDSDK_fp_GetProcessorStockBusFrequency)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorStockBusFrequency));
                    result = (float)cpuidsdk_fp_GetProcessorStockBusFrequency(this.objptr, _proc_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004C4 RID: 1220 RVA: 0x0002EBEC File Offset: 0x0002CDEC
        public int GetProcessorMaxCacheLevel(int _proc_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3666326799u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorMaxCacheLevel cpuidsdk_fp_GetProcessorMaxCacheLevel = (CPUIDSDK.CPUIDSDK_fp_GetProcessorMaxCacheLevel)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorMaxCacheLevel));
                    result = cpuidsdk_fp_GetProcessorMaxCacheLevel(this.objptr, _proc_index);
                }
                else
                {
                    result = 0;
                }
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        // Token: 0x060004C5 RID: 1221 RVA: 0x0002EC60 File Offset: 0x0002CE60
        public void GetProcessorCacheParameters(int _proc_index, int _cache_level, int _cache_type, ref int _NbCaches, ref int _size)
        {
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1443015685u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorCacheParameters cpuidsdk_fp_GetProcessorCacheParameters = (CPUIDSDK.CPUIDSDK_fp_GetProcessorCacheParameters)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorCacheParameters));
                    cpuidsdk_fp_GetProcessorCacheParameters(this.objptr, _proc_index, _cache_level, _cache_type, ref _NbCaches, ref _size);
                }
            }
            catch
            {
            }
        }

        // Token: 0x060004C6 RID: 1222 RVA: 0x0002ECD4 File Offset: 0x0002CED4
        public uint GetProcessorID(int _proc_index)
        {
            uint result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(504904752u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorID cpuidsdk_fp_GetProcessorID = (CPUIDSDK.CPUIDSDK_fp_GetProcessorID)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorID));
                    result = (uint)cpuidsdk_fp_GetProcessorID(this.objptr, _proc_index);
                }
                else
                {
                    result = 0u;
                }
            }
            catch
            {
                result = 0u;
            }
            return result;
        }

        // Token: 0x060004C7 RID: 1223 RVA: 0x0002ED48 File Offset: 0x0002CF48
        public float GetProcessorVoltageID(int _proc_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(4267441335u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorVoltageID cpuidsdk_fp_GetProcessorVoltageID = (CPUIDSDK.CPUIDSDK_fp_GetProcessorVoltageID)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorVoltageID));
                    result = cpuidsdk_fp_GetProcessorVoltageID(this.objptr, _proc_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004C8 RID: 1224 RVA: 0x0002EDC4 File Offset: 0x0002CFC4
        public int GetMemoryType()
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2529111422u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMemoryType cpuidsdk_fp_GetMemoryType = (CPUIDSDK.CPUIDSDK_fp_GetMemoryType)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMemoryType));
                    result = cpuidsdk_fp_GetMemoryType(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004C9 RID: 1225 RVA: 0x0002EE3C File Offset: 0x0002D03C
        public int GetMemorySize()
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2518625342u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMemorySize cpuidsdk_fp_GetMemorySize = (CPUIDSDK.CPUIDSDK_fp_GetMemorySize)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMemorySize));
                    result = cpuidsdk_fp_GetMemorySize(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004CA RID: 1226 RVA: 0x0002EEB4 File Offset: 0x0002D0B4
        public float GetMemoryClockFrequency()
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(256384656u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMemoryClockFrequency cpuidsdk_fp_GetMemoryClockFrequency = (CPUIDSDK.CPUIDSDK_fp_GetMemoryClockFrequency)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMemoryClockFrequency));
                    result = cpuidsdk_fp_GetMemoryClockFrequency(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004CB RID: 1227 RVA: 0x0002EF30 File Offset: 0x0002D130
        public int GetMemoryNumberOfChannels()
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3296430327u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMemoryNumberOfChannels cpuidsdk_fp_GetMemoryNumberOfChannels = (CPUIDSDK.CPUIDSDK_fp_GetMemoryNumberOfChannels)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMemoryNumberOfChannels));
                    result = cpuidsdk_fp_GetMemoryNumberOfChannels(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004CC RID: 1228 RVA: 0x0002EFA8 File Offset: 0x0002D1A8
        public float GetMemoryCASLatency()
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3075042962u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMemoryCASLatency cpuidsdk_fp_GetMemoryCASLatency = (CPUIDSDK.CPUIDSDK_fp_GetMemoryCASLatency)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMemoryCASLatency));
                    result = cpuidsdk_fp_GetMemoryCASLatency(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004CD RID: 1229 RVA: 0x0002F024 File Offset: 0x0002D224
        public int GetMemoryRAStoCASDelay()
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(206051472u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMemoryRAStoCASDelay cpuidsdk_fp_GetMemoryRAStoCASDelay = (CPUIDSDK.CPUIDSDK_fp_GetMemoryRAStoCASDelay)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMemoryRAStoCASDelay));
                    result = cpuidsdk_fp_GetMemoryRAStoCASDelay(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004CE RID: 1230 RVA: 0x0002F09C File Offset: 0x0002D29C
        public int GetMemoryRASPrecharge()
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1215598825u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMemoryRASPrecharge cpuidsdk_fp_GetMemoryRASPrecharge = (CPUIDSDK.CPUIDSDK_fp_GetMemoryRASPrecharge)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMemoryRASPrecharge));
                    result = cpuidsdk_fp_GetMemoryRASPrecharge(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004CF RID: 1231 RVA: 0x0002F114 File Offset: 0x0002D314
        public int GetMemoryTRAS()
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1866522241u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMemoryTRAS cpuidsdk_fp_GetMemoryTRAS = (CPUIDSDK.CPUIDSDK_fp_GetMemoryTRAS)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMemoryTRAS));
                    result = cpuidsdk_fp_GetMemoryTRAS(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004D0 RID: 1232 RVA: 0x0002F18C File Offset: 0x0002D38C
        public int GetMemoryTRC()
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(771513336u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMemoryTRC cpuidsdk_fp_GetMemoryTRC = (CPUIDSDK.CPUIDSDK_fp_GetMemoryTRC)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMemoryTRC));
                    result = cpuidsdk_fp_GetMemoryTRC(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004D1 RID: 1233 RVA: 0x0002F204 File Offset: 0x0002D404
        public int GetMemoryCommandRate()
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3899117775u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMemoryCommandRate cpuidsdk_fp_GetMemoryCommandRate = (CPUIDSDK.CPUIDSDK_fp_GetMemoryCommandRate)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMemoryCommandRate));
                    result = cpuidsdk_fp_GetMemoryCommandRate(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004D2 RID: 1234 RVA: 0x0002F27C File Offset: 0x0002D47C
        public string GetNorthBridgeVendor()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1827461593u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetNorthBridgeVendor cpuidsdk_fp_GetNorthBridgeVendor = (CPUIDSDK.CPUIDSDK_fp_GetNorthBridgeVendor)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetNorthBridgeVendor));
                    IntPtr ptr = cpuidsdk_fp_GetNorthBridgeVendor(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004D3 RID: 1235 RVA: 0x0002F304 File Offset: 0x0002D504
        public string GetNorthBridgeModel()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1952114869u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetNorthBridgeModel cpuidsdk_fp_GetNorthBridgeModel = (CPUIDSDK.CPUIDSDK_fp_GetNorthBridgeModel)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetNorthBridgeModel));
                    IntPtr ptr = cpuidsdk_fp_GetNorthBridgeModel(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004D4 RID: 1236 RVA: 0x0002F38C File Offset: 0x0002D58C
        public string GetNorthBridgeRevision()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3822307239u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetNorthBridgeRevision cpuidsdk_fp_GetNorthBridgeRevision = (CPUIDSDK.CPUIDSDK_fp_GetNorthBridgeRevision)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetNorthBridgeRevision));
                    IntPtr ptr = cpuidsdk_fp_GetNorthBridgeRevision(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004D5 RID: 1237 RVA: 0x0002F414 File Offset: 0x0002D614
        public string GetSouthBridgeVendor()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(4054180683u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSouthBridgeVendor cpuidsdk_fp_GetSouthBridgeVendor = (CPUIDSDK.CPUIDSDK_fp_GetSouthBridgeVendor)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSouthBridgeVendor));
                    IntPtr ptr = cpuidsdk_fp_GetSouthBridgeVendor(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004D6 RID: 1238 RVA: 0x0002F49C File Offset: 0x0002D69C
        public string GetSouthBridgeModel()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1021344192u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSouthBridgeModel cpuidsdk_fp_GetSouthBridgeModel = (CPUIDSDK.CPUIDSDK_fp_GetSouthBridgeModel)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSouthBridgeModel));
                    IntPtr ptr = cpuidsdk_fp_GetSouthBridgeModel(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004D7 RID: 1239 RVA: 0x0002F524 File Offset: 0x0002D724
        public string GetSouthBridgeRevision()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(964588284u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSouthBridgeRevision cpuidsdk_fp_GetSouthBridgeRevision = (CPUIDSDK.CPUIDSDK_fp_GetSouthBridgeRevision)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSouthBridgeRevision));
                    IntPtr ptr = cpuidsdk_fp_GetSouthBridgeRevision(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004D8 RID: 1240 RVA: 0x0002F5AC File Offset: 0x0002D7AC
        public void GetGraphicBusLinkParameters(ref int _bus_type, ref int _link_width)
        {
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(0u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetGraphicBusLinkParameters cpuidsdk_fp_GetGraphicBusLinkParameters = (CPUIDSDK.CPUIDSDK_fp_GetGraphicBusLinkParameters)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetGraphicBusLinkParameters));
                    cpuidsdk_fp_GetGraphicBusLinkParameters(this.objptr, ref _bus_type, ref _link_width);
                }
            }
            catch
            {
            }
        }

        // Token: 0x060004D9 RID: 1241 RVA: 0x0002F618 File Offset: 0x0002D818
        public void GetMemorySlotsConfig(ref int _nbslots, ref int _nbusedslots, ref int _slotmap_h, ref int _slotmap_l, ref int _maxmodulesize)
        {
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(869689260u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMemorySlotsConfig cpuidsdk_fp_GetMemorySlotsConfig = (CPUIDSDK.CPUIDSDK_fp_GetMemorySlotsConfig)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMemorySlotsConfig));
                    cpuidsdk_fp_GetMemorySlotsConfig(this.objptr, ref _nbslots, ref _nbusedslots, ref _slotmap_h, ref _slotmap_l, ref _maxmodulesize);
                }
            }
            catch
            {
            }
        }

        // Token: 0x060004DA RID: 1242 RVA: 0x0002F68C File Offset: 0x0002D88C
        public string GetBIOSVendor()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2265124358u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetBIOSVendor cpuidsdk_fp_GetBIOSVendor = (CPUIDSDK.CPUIDSDK_fp_GetBIOSVendor)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetBIOSVendor));
                    IntPtr ptr = cpuidsdk_fp_GetBIOSVendor(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004DB RID: 1243 RVA: 0x0002F714 File Offset: 0x0002D914
        public string GetBIOSVersion()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1623769489u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetBIOSVersion cpuidsdk_fp_GetBIOSVersion = (CPUIDSDK.CPUIDSDK_fp_GetBIOSVersion)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetBIOSVersion));
                    IntPtr ptr = cpuidsdk_fp_GetBIOSVersion(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004DC RID: 1244 RVA: 0x0002F79C File Offset: 0x0002D99C
        public string GetBIOSDate()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1198952173u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetBIOSDate cpuidsdk_fp_GetBIOSDate = (CPUIDSDK.CPUIDSDK_fp_GetBIOSDate)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetBIOSDate));
                    IntPtr ptr = cpuidsdk_fp_GetBIOSDate(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004DD RID: 1245 RVA: 0x0002F824 File Offset: 0x0002DA24
        public string GetMainboardVendor()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1914364981u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMainboardVendor cpuidsdk_fp_GetMainboardVendor = (CPUIDSDK.CPUIDSDK_fp_GetMainboardVendor)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMainboardVendor));
                    IntPtr ptr = cpuidsdk_fp_GetMainboardVendor(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004DE RID: 1246 RVA: 0x0002F8AC File Offset: 0x0002DAAC
        public string GetMainboardModel()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1325440513u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMainboardModel cpuidsdk_fp_GetMainboardModel = (CPUIDSDK.CPUIDSDK_fp_GetMainboardModel)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMainboardModel));
                    IntPtr ptr = cpuidsdk_fp_GetMainboardModel(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004DF RID: 1247 RVA: 0x0002F934 File Offset: 0x0002DB34
        public string GetMainboardRevision()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1789318477u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMainboardRevision cpuidsdk_fp_GetMainboardRevision = (CPUIDSDK.CPUIDSDK_fp_GetMainboardRevision)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMainboardRevision));
                    IntPtr ptr = cpuidsdk_fp_GetMainboardRevision(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004E0 RID: 1248 RVA: 0x0002F9BC File Offset: 0x0002DBBC
        public string GetMainboardSerialNumber()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3882340047u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMainboardSerialNumber cpuidsdk_fp_GetMainboardSerialNumber = (CPUIDSDK.CPUIDSDK_fp_GetMainboardSerialNumber)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMainboardSerialNumber));
                    IntPtr ptr = cpuidsdk_fp_GetMainboardSerialNumber(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004E1 RID: 1249 RVA: 0x0002FA44 File Offset: 0x0002DC44
        public string GetSystemManufacturer()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3580734171u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSystemManufacturer cpuidsdk_fp_GetSystemManufacturer = (CPUIDSDK.CPUIDSDK_fp_GetSystemManufacturer)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSystemManufacturer));
                    IntPtr ptr = cpuidsdk_fp_GetSystemManufacturer(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004E2 RID: 1250 RVA: 0x0002FACC File Offset: 0x0002DCCC
        public string GetSystemProductName()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2569220678u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSystemProductName cpuidsdk_fp_GetSystemProductName = (CPUIDSDK.CPUIDSDK_fp_GetSystemProductName)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSystemProductName));
                    IntPtr ptr = cpuidsdk_fp_GetSystemProductName(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004E3 RID: 1251 RVA: 0x0002FB54 File Offset: 0x0002DD54
        public string GetSystemVersion()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3680876235u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSystemVersion cpuidsdk_fp_GetSystemVersion = (CPUIDSDK.CPUIDSDK_fp_GetSystemVersion)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSystemVersion));
                    IntPtr ptr = cpuidsdk_fp_GetSystemVersion(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004E4 RID: 1252 RVA: 0x0002FBDC File Offset: 0x0002DDDC
        public string GetSystemSerialNumber()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2422939862u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSystemSerialNumber cpuidsdk_fp_GetSystemSerialNumber = (CPUIDSDK.CPUIDSDK_fp_GetSystemSerialNumber)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSystemSerialNumber));
                    IntPtr ptr = cpuidsdk_fp_GetSystemSerialNumber(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004E5 RID: 1253 RVA: 0x0002FC64 File Offset: 0x0002DE64
        public string GetSystemUUID()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1717488829u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSystemUUID cpuidsdk_fp_GetSystemUUID = (CPUIDSDK.CPUIDSDK_fp_GetSystemUUID)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSystemUUID));
                    IntPtr ptr = cpuidsdk_fp_GetSystemUUID(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004E6 RID: 1254 RVA: 0x0002FCEC File Offset: 0x0002DEEC
        public string GetChassisManufacturer()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1414834345u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetChassisManufacturer cpuidsdk_fp_GetChassisManufacturer = (CPUIDSDK.CPUIDSDK_fp_GetChassisManufacturer)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetChassisManufacturer));
                    IntPtr ptr = cpuidsdk_fp_GetChassisManufacturer(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004E7 RID: 1255 RVA: 0x0002FD74 File Offset: 0x0002DF74
        public string GetChassisType()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1484828929u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetChassisType cpuidsdk_fp_GetChassisType = (CPUIDSDK.CPUIDSDK_fp_GetChassisType)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetChassisType));
                    IntPtr ptr = cpuidsdk_fp_GetChassisType(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004E8 RID: 1256 RVA: 0x0002FDFC File Offset: 0x0002DFFC
        public string GetChassisSerialNumber()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1968761521u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetChassisSerialNumber cpuidsdk_fp_GetChassisSerialNumber = (CPUIDSDK.CPUIDSDK_fp_GetChassisSerialNumber)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetChassisSerialNumber));
                    IntPtr ptr = cpuidsdk_fp_GetChassisSerialNumber(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004E9 RID: 1257 RVA: 0x0002FE84 File Offset: 0x0002E084
        public bool GetMemoryInfosExt(ref string _szLocation, ref string _szUsage, ref string _szCorrection)
        {
            bool result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1041136668u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    IntPtr zero = IntPtr.Zero;
                    IntPtr zero2 = IntPtr.Zero;
                    IntPtr zero3 = IntPtr.Zero;
                    CPUIDSDK.CPUIDSDK_fp_GetMemoryInfosExt cpuidsdk_fp_GetMemoryInfosExt = (CPUIDSDK.CPUIDSDK_fp_GetMemoryInfosExt)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMemoryInfosExt));
                    int num = cpuidsdk_fp_GetMemoryInfosExt(this.objptr, ref zero, ref zero2, ref zero3);
                    bool flag2 = num == 1;
                    if (flag2)
                    {
                        _szLocation = Marshal.PtrToStringAnsi(zero);
                        Marshal.FreeBSTR(zero);
                        _szUsage = Marshal.PtrToStringAnsi(zero2);
                        Marshal.FreeBSTR(zero2);
                        _szCorrection = Marshal.PtrToStringAnsi(zero3);
                        Marshal.FreeBSTR(zero3);
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        // Token: 0x060004EA RID: 1258 RVA: 0x0002FF5C File Offset: 0x0002E15C
        public int GetNumberOfMemoryDevices()
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1571732317u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetNumberOfMemoryDevices cpuidsdk_fp_GetNumberOfMemoryDevices = (CPUIDSDK.CPUIDSDK_fp_GetNumberOfMemoryDevices)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetNumberOfMemoryDevices));
                    result = cpuidsdk_fp_GetNumberOfMemoryDevices(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004EB RID: 1259 RVA: 0x0002FFD4 File Offset: 0x0002E1D4
        public bool GetMemoryDeviceInfos(int _device_index, ref int _size, ref string _szFormat)
        {
            bool result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1916986501u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    IntPtr zero = IntPtr.Zero;
                    CPUIDSDK.CPUIDSDK_fp_GetMemoryDeviceInfos cpuidsdk_fp_GetMemoryDeviceInfos = (CPUIDSDK.CPUIDSDK_fp_GetMemoryDeviceInfos)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMemoryDeviceInfos));
                    int num = cpuidsdk_fp_GetMemoryDeviceInfos(this.objptr, _device_index, ref _size, ref zero);
                    bool flag2 = num == 1;
                    if (flag2)
                    {
                        _szFormat = Marshal.PtrToStringAnsi(zero);
                        Marshal.FreeBSTR(zero);
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        // Token: 0x060004EC RID: 1260 RVA: 0x00030078 File Offset: 0x0002E278
        public bool GetMemoryDeviceInfosExt(int _device_index, ref string _szDesignation, ref string _szType, ref int _total_width, ref int _data_width, ref int _speed)
        {
            bool result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2754824294u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    IntPtr zero = IntPtr.Zero;
                    IntPtr zero2 = IntPtr.Zero;
                    CPUIDSDK.CPUIDSDK_fp_GetMemoryDeviceInfosExt cpuidsdk_fp_GetMemoryDeviceInfosExt = (CPUIDSDK.CPUIDSDK_fp_GetMemoryDeviceInfosExt)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMemoryDeviceInfosExt));
                    int num = cpuidsdk_fp_GetMemoryDeviceInfosExt(this.objptr, _device_index, ref zero, ref zero2, ref _total_width, ref _data_width, ref _speed);
                    bool flag2 = num == 1;
                    if (flag2)
                    {
                        _szDesignation = Marshal.PtrToStringAnsi(zero);
                        Marshal.FreeBSTR(zero);
                        _szType = Marshal.PtrToStringAnsi(zero2);
                        Marshal.FreeBSTR(zero2);
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        // Token: 0x060004ED RID: 1261 RVA: 0x00030138 File Offset: 0x0002E338
        public int GetProcessorSockets()
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(354691656u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetProcessorSockets cpuidsdk_fp_GetProcessorSockets = (CPUIDSDK.CPUIDSDK_fp_GetProcessorSockets)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetProcessorSockets));
                    result = cpuidsdk_fp_GetProcessorSockets(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004EE RID: 1262 RVA: 0x000301B0 File Offset: 0x0002E3B0
        public string[] GetSPDModulesManufacturer()
        {
            int numberOfSPDModules = this.GetNumberOfSPDModules();
            List<string> list = new List<string>();
            for (int i = 0; i < numberOfSPDModules; i++)
            {
                list.Add(this.GetSPDModuleManufacturer(i));
            }
            return list.ToArray();
        }

        // Token: 0x060004EF RID: 1263 RVA: 0x000301F8 File Offset: 0x0002E3F8
        public float GetSPDModulesVDD(int index)
        {
            int numberOfSPDModules = this.GetNumberOfSPDModules();
            float num = 0f;
            float num2 = 0f;
            float num3 = 0f;
            int num4 = 0;
            float result;
            if (num4 >= numberOfSPDModules)
            {
                result = -1f;
            }
            else
            {
                this.GetSPDModuleProfileInfos(num4, 0, ref num, ref num2, ref num3);
                result = num3;
            }
            return result;
        }

        // Token: 0x060004F0 RID: 1264 RVA: 0x00030250 File Offset: 0x0002E450
        public float GetSPDModulesFrequency()
        {
            int numberOfSPDModules = this.GetNumberOfSPDModules();
            float num = 0f;
            float num2 = 0f;
            float num3 = 0f;
            int num4 = 0;
            float result;
            if (num4 >= numberOfSPDModules)
            {
                result = -1f;
            }
            else
            {
                this.GetSPDModuleProfileInfos(num4, 0, ref num, ref num2, ref num3);
                result = num;
            }
            return result;
        }

        // Token: 0x060004F1 RID: 1265 RVA: 0x000302A8 File Offset: 0x0002E4A8
        public int GetNumberOfSPDModules()
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1178897545u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetNumberOfSPDModules cpuidsdk_fp_GetNumberOfSPDModules = (CPUIDSDK.CPUIDSDK_fp_GetNumberOfSPDModules)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetNumberOfSPDModules));
                    result = cpuidsdk_fp_GetNumberOfSPDModules(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004F2 RID: 1266 RVA: 0x00030320 File Offset: 0x0002E520
        public int GetSPDModuleType(int _spd_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1527166477u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleType cpuidsdk_fp_GetSPDModuleType = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleType)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleType));
                    result = cpuidsdk_fp_GetSPDModuleType(this.objptr, _spd_index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004F3 RID: 1267 RVA: 0x0003039C File Offset: 0x0002E59C
        public int GetSPDModuleSize(int _spd_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1826544061u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleSize cpuidsdk_fp_GetSPDModuleSize = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleSize)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleSize));
                    result = cpuidsdk_fp_GetSPDModuleSize(this.objptr, _spd_index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004F4 RID: 1268 RVA: 0x00030418 File Offset: 0x0002E618
        public string GetSPDModuleFormat(int _spd_index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2125135189u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleFormat cpuidsdk_fp_GetSPDModuleFormat = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleFormat)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleFormat));
                    IntPtr ptr = cpuidsdk_fp_GetSPDModuleFormat(this.objptr, _spd_index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004F5 RID: 1269 RVA: 0x000304A0 File Offset: 0x0002E6A0
        public string GetSPDModuleManufacturer(int _spd_index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3699882255u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleManufacturer cpuidsdk_fp_GetSPDModuleManufacturer = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleManufacturer)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleManufacturer));
                    IntPtr ptr = cpuidsdk_fp_GetSPDModuleManufacturer(this.objptr, _spd_index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004F6 RID: 1270 RVA: 0x00030528 File Offset: 0x0002E728
        public bool GetSPDModuleManufacturerID(int _spd_index, byte[] _id)
        {
            bool result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1672529761u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleManufacturerID cpuidsdk_fp_GetSPDModuleManufacturerID = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleManufacturerID)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleManufacturerID));
                    int num = cpuidsdk_fp_GetSPDModuleManufacturerID(this.objptr, _spd_index, _id);
                    bool flag2 = num == 1;
                    if (flag2)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        // Token: 0x060004F7 RID: 1271 RVA: 0x000305B0 File Offset: 0x0002E7B0
        public int GetSPDModuleMaxFrequency(int _spd_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(278667576u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleMaxFrequency cpuidsdk_fp_GetSPDModuleMaxFrequency = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleMaxFrequency)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleMaxFrequency));
                    result = cpuidsdk_fp_GetSPDModuleMaxFrequency(this.objptr, _spd_index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004F8 RID: 1272 RVA: 0x0003062C File Offset: 0x0002E82C
        public string GetSPDModuleSpecification(int _spd_index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2498570714u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleSpecification cpuidsdk_fp_GetSPDModuleSpecification = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleSpecification)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleSpecification));
                    IntPtr ptr = cpuidsdk_fp_GetSPDModuleSpecification(this.objptr, _spd_index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004F9 RID: 1273 RVA: 0x000306B4 File Offset: 0x0002E8B4
        public string GetSPDModulePartNumber(int _spd_index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3990870975u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModulePartNumber cpuidsdk_fp_GetSPDModulePartNumber = (CPUIDSDK.CPUIDSDK_fp_GetSPDModulePartNumber)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModulePartNumber));
                    IntPtr ptr = cpuidsdk_fp_GetSPDModulePartNumber(this.objptr, _spd_index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004FA RID: 1274 RVA: 0x0003073C File Offset: 0x0002E93C
        public string GetSPDModuleSerialNumber(int _spd_index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2437751450u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleSerialNumber cpuidsdk_fp_GetSPDModuleSerialNumber = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleSerialNumber)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleSerialNumber));
                    IntPtr ptr = cpuidsdk_fp_GetSPDModuleSerialNumber(this.objptr, _spd_index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x060004FB RID: 1275 RVA: 0x000307C4 File Offset: 0x0002E9C4
        public float GetSPDModuleMinTRCD(int _spd_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(159781644u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleMinTRCD cpuidsdk_fp_GetSPDModuleMinTRCD = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleMinTRCD)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleMinTRCD));
                    result = cpuidsdk_fp_GetSPDModuleMinTRCD(this.objptr, _spd_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004FC RID: 1276 RVA: 0x00030840 File Offset: 0x0002EA40
        public float GetSPDModuleMinTRP(int _spd_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(881879328u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleMinTRP cpuidsdk_fp_GetSPDModuleMinTRP = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleMinTRP)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleMinTRP));
                    result = cpuidsdk_fp_GetSPDModuleMinTRP(this.objptr, _spd_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004FD RID: 1277 RVA: 0x000308BC File Offset: 0x0002EABC
        public float GetSPDModuleMinTRAS(int _spd_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(4202689791u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleMinTRAS cpuidsdk_fp_GetSPDModuleMinTRAS = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleMinTRAS)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleMinTRAS));
                    result = cpuidsdk_fp_GetSPDModuleMinTRAS(this.objptr, _spd_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004FE RID: 1278 RVA: 0x00030938 File Offset: 0x0002EB38
        public float GetSPDModuleMinTRC(int _spd_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(4281335391u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleMinTRC cpuidsdk_fp_GetSPDModuleMinTRC = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleMinTRC)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleMinTRC));
                    result = cpuidsdk_fp_GetSPDModuleMinTRC(this.objptr, _spd_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x060004FF RID: 1279 RVA: 0x000309B4 File Offset: 0x0002EBB4
        public int GetSPDModuleManufacturingDate(int _spd_index, ref int _year, ref int _week)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2990367866u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleManufacturingDate cpuidsdk_fp_GetSPDModuleManufacturingDate = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleManufacturingDate)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleManufacturingDate));
                    result = cpuidsdk_fp_GetSPDModuleManufacturingDate(this.objptr, _spd_index, ref _year, ref _week);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000500 RID: 1280 RVA: 0x00030A30 File Offset: 0x0002EC30
        public int GetSPDModuleNumberOfBanks(int _spd_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3672880599u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleNumberOfBanks cpuidsdk_fp_GetSPDModuleNumberOfBanks = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleNumberOfBanks)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleNumberOfBanks));
                    result = cpuidsdk_fp_GetSPDModuleNumberOfBanks(this.objptr, _spd_index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000501 RID: 1281 RVA: 0x00030AAC File Offset: 0x0002ECAC
        public int GetSPDModuleDataWidth(int _spd_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(4034912511u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleDataWidth cpuidsdk_fp_GetSPDModuleDataWidth = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleDataWidth)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleDataWidth));
                    result = cpuidsdk_fp_GetSPDModuleDataWidth(this.objptr, _spd_index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000502 RID: 1282 RVA: 0x00030B28 File Offset: 0x0002ED28
        public int GetSPDModuleNumberOfProfiles(int _spd_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(30933936u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleNumberOfProfiles cpuidsdk_fp_GetSPDModuleNumberOfProfiles = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleNumberOfProfiles)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleNumberOfProfiles));
                    result = cpuidsdk_fp_GetSPDModuleNumberOfProfiles(this.objptr, _spd_index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000503 RID: 1283 RVA: 0x00030BA4 File Offset: 0x0002EDA4
        public void GetSPDModuleProfileInfos(int _spd_index, int _profile_index, ref float _frequency, ref float _tCL, ref float _nominal_vdd)
        {
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(167384052u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleProfileInfos cpuidsdk_fp_GetSPDModuleProfileInfos = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleProfileInfos)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleProfileInfos));
                    cpuidsdk_fp_GetSPDModuleProfileInfos(this.objptr, _spd_index, _profile_index, ref _frequency, ref _tCL, ref _nominal_vdd);
                }
            }
            catch
            {
            }
        }

        // Token: 0x06000504 RID: 1284 RVA: 0x00030C18 File Offset: 0x0002EE18
        public int GetSPDModuleNumberOfEPPProfiles(int _spd_index, ref int _epp_revision)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3780493995u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleNumberOfEPPProfiles cpuidsdk_fp_GetSPDModuleNumberOfEPPProfiles = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleNumberOfEPPProfiles)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleNumberOfEPPProfiles));
                    result = cpuidsdk_fp_GetSPDModuleNumberOfEPPProfiles(this.objptr, _spd_index, ref _epp_revision);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000505 RID: 1285 RVA: 0x00030C94 File Offset: 0x0002EE94
        public void GetSPDModuleEPPProfileInfos(int _spd_index, int _profile_index, ref float _frequency, ref float _tCL, ref float _tRCD, ref float _tRAS, ref float _tRP, ref float _tRC, ref float _nominal_vdd)
        {
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1019115900u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleEPPProfileInfos cpuidsdk_fp_GetSPDModuleEPPProfileInfos = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleEPPProfileInfos)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleEPPProfileInfos));
                    cpuidsdk_fp_GetSPDModuleEPPProfileInfos(this.objptr, _spd_index, _profile_index, ref _frequency, ref _tCL, ref _tRCD, ref _tRAS, ref _tRP, ref _tRC, ref _nominal_vdd);
                }
            }
            catch
            {
            }
        }

        // Token: 0x06000506 RID: 1286 RVA: 0x00030D10 File Offset: 0x0002EF10
        public int GetSPDModuleNumberOfXMPProfiles(int _spd_index, ref int _xmp_revision)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1046248632u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleNumberOfXMPProfiles cpuidsdk_fp_GetSPDModuleNumberOfXMPProfiles = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleNumberOfXMPProfiles)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleNumberOfXMPProfiles));
                    result = cpuidsdk_fp_GetSPDModuleNumberOfXMPProfiles(this.objptr, _spd_index, ref _xmp_revision);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000507 RID: 1287 RVA: 0x00030D8C File Offset: 0x0002EF8C
        public int GetSPDModuleXMPProfileNumberOfCL(int _spd_index, int _profile_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3610095195u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleXMPProfileNumberOfCL cpuidsdk_fp_GetSPDModuleXMPProfileNumberOfCL = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleXMPProfileNumberOfCL)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleXMPProfileNumberOfCL));
                    result = cpuidsdk_fp_GetSPDModuleXMPProfileNumberOfCL(this.objptr, _spd_index, _profile_index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000508 RID: 1288 RVA: 0x00030E08 File Offset: 0x0002F008
        public void GetSPDModuleXMPProfileCLInfos(int _spd_index, int _profile_index, int _cl_index, ref float _frequency, ref float _CL)
        {
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1327930957u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleXMPProfileCLInfos cpuidsdk_fp_GetSPDModuleXMPProfileCLInfos = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleXMPProfileCLInfos)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleXMPProfileCLInfos));
                    cpuidsdk_fp_GetSPDModuleXMPProfileCLInfos(this.objptr, _spd_index, _profile_index, _cl_index, ref _frequency, ref _CL);
                }
            }
            catch
            {
            }
        }

        // Token: 0x06000509 RID: 1289 RVA: 0x00030E7C File Offset: 0x0002F07C
        public void GetSPDModuleXMPProfileInfos(int _spd_index, int _profile_index, ref float _tRCD, ref float _tRAS, ref float _tRP, ref float _nominal_vdd, ref int _max_freq, ref float _max_CL)
        {
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2764261766u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleXMPProfileInfos cpuidsdk_fp_GetSPDModuleXMPProfileInfos = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleXMPProfileInfos)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleXMPProfileInfos));
                    cpuidsdk_fp_GetSPDModuleXMPProfileInfos(this.objptr, _spd_index, _profile_index, ref _tRCD, ref _tRAS, ref _tRP, ref _nominal_vdd, ref _max_freq, ref _max_CL);
                }
            }
            catch
            {
            }
        }

        // Token: 0x0600050A RID: 1290 RVA: 0x00030EF8 File Offset: 0x0002F0F8
        public int GetSPDModuleNumberOfAMPProfiles(int _spd_index, ref int _amp_revision)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2286227594u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleNumberOfAMPProfiles cpuidsdk_fp_GetSPDModuleNumberOfAMPProfiles = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleNumberOfAMPProfiles)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleNumberOfAMPProfiles));
                    result = cpuidsdk_fp_GetSPDModuleNumberOfAMPProfiles(this.objptr, _spd_index, ref _amp_revision);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x0600050B RID: 1291 RVA: 0x00030F74 File Offset: 0x0002F174
        public void GetSPDModuleAMPProfileInfos(int _spd_index, int _profile_index, ref int _frequency, ref float _min_cycle_time, ref float _tCL, ref float _tRCD, ref float _tRAS, ref float _tRP, ref float _tRC)
        {
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3124196462u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleAMPProfileInfos cpuidsdk_fp_GetSPDModuleAMPProfileInfos = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleAMPProfileInfos)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleAMPProfileInfos));
                    cpuidsdk_fp_GetSPDModuleAMPProfileInfos(this.objptr, _spd_index, _profile_index, ref _frequency, ref _min_cycle_time, ref _tCL, ref _tRCD, ref _tRAS, ref _tRP, ref _tRC);
                }
            }
            catch
            {
            }
        }

        // Token: 0x0600050C RID: 1292 RVA: 0x00030FF0 File Offset: 0x0002F1F0
        public int GetSPDModuleRawData(int _spd_index, int _offset)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2991940778u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSPDModuleRawData cpuidsdk_fp_GetSPDModuleRawData = (CPUIDSDK.CPUIDSDK_fp_GetSPDModuleRawData)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSPDModuleRawData));
                    result = cpuidsdk_fp_GetSPDModuleRawData(this.objptr, _spd_index, _offset);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x0600050D RID: 1293 RVA: 0x0003106C File Offset: 0x0002F26C
        public int GetNumberOfDisplayAdapter()
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3205856810u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetNumberOfDisplayAdapter cpuidsdk_fp_GetNumberOfDisplayAdapter = (CPUIDSDK.CPUIDSDK_fp_GetNumberOfDisplayAdapter)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetNumberOfDisplayAdapter));
                    result = cpuidsdk_fp_GetNumberOfDisplayAdapter(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x0600050E RID: 1294 RVA: 0x000310E4 File Offset: 0x0002F2E4
        public string GetDisplayAdapterName(int _adapter_index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2148073490u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterName cpuidsdk_fp_GetDisplayAdapterName = (CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterName)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterName));
                    IntPtr ptr = cpuidsdk_fp_GetDisplayAdapterName(this.objptr, _adapter_index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x0600050F RID: 1295 RVA: 0x0003116C File Offset: 0x0002F36C
        public string GetDisplayAdapterCodeName(int _adapter_index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(337389624u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterCodeName cpuidsdk_fp_GetDisplayAdapterCodeName = (CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterCodeName)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterCodeName));
                    IntPtr ptr = cpuidsdk_fp_GetDisplayAdapterCodeName(this.objptr, _adapter_index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x06000510 RID: 1296 RVA: 0x000311F4 File Offset: 0x0002F3F4
        public int GetDisplayAdapterNumberOfPerformanceLevels(int _adapter_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(4142263755u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterNumberOfPerformanceLevels cpuidsdk_fp_GetDisplayAdapterNumberOfPerformanceLevels = (CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterNumberOfPerformanceLevels)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterNumberOfPerformanceLevels));
                    result = cpuidsdk_fp_GetDisplayAdapterNumberOfPerformanceLevels(this.objptr, _adapter_index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000511 RID: 1297 RVA: 0x00031270 File Offset: 0x0002F470
        public int GetDisplayAdapterCurrentPerformanceLevel(int _adapter_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(526401216u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterCurrentPerformanceLevel cpuidsdk_fp_GetDisplayAdapterCurrentPerformanceLevel = (CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterCurrentPerformanceLevel)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterCurrentPerformanceLevel));
                    result = cpuidsdk_fp_GetDisplayAdapterCurrentPerformanceLevel(this.objptr, _adapter_index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000512 RID: 1298 RVA: 0x000312EC File Offset: 0x0002F4EC
        public string GetDisplayAdapterPerformanceLevelName(int _adapter_index, int _perf_level)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(700076916u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterPerformanceLevelName cpuidsdk_fp_GetDisplayAdapterPerformanceLevelName = (CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterPerformanceLevelName)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterPerformanceLevelName));
                    IntPtr ptr = cpuidsdk_fp_GetDisplayAdapterPerformanceLevelName(this.objptr, _adapter_index, _perf_level);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x06000513 RID: 1299 RVA: 0x00031374 File Offset: 0x0002F574
        public float GetDisplayAdapterClock(int _adapter_index, int _perf_level, int _domain)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2573546186u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterClock cpuidsdk_fp_GetDisplayAdapterClock = (CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterClock)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterClock));
                    result = cpuidsdk_fp_GetDisplayAdapterClock(this.objptr, _adapter_index, _perf_level, _domain);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000514 RID: 1300 RVA: 0x000313F4 File Offset: 0x0002F5F4
        public float GetDisplayAdapterStockClock(int _adapter_index, int _perf_level, int _domain)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1713163321u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterStockClock cpuidsdk_fp_GetDisplayAdapterStockClock = (CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterStockClock)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterStockClock));
                    result = cpuidsdk_fp_GetDisplayAdapterStockClock(this.objptr, _adapter_index, _perf_level, _domain);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000515 RID: 1301 RVA: 0x00031474 File Offset: 0x0002F674
        public float GetDisplayAdapterManufacturingProcess(int _adapter_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(4154191671u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterManufacturingProcess cpuidsdk_fp_GetDisplayAdapterManufacturingProcess = (CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterManufacturingProcess)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterManufacturingProcess));
                    result = cpuidsdk_fp_GetDisplayAdapterManufacturingProcess(this.objptr, _adapter_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000516 RID: 1302 RVA: 0x000314F0 File Offset: 0x0002F6F0
        public float GetDisplayAdapterTemperature(int _adapter_index, int _domain)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1170901909u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterTemperature cpuidsdk_fp_GetDisplayAdapterTemperature = (CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterTemperature)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterTemperature));
                    result = cpuidsdk_fp_GetDisplayAdapterTemperature(this.objptr, _adapter_index, _domain);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000517 RID: 1303 RVA: 0x0003156C File Offset: 0x0002F76C
        public int GetDisplayAdapterFanSpeed(int _adapter_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1972300573u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterFanSpeed cpuidsdk_fp_GetDisplayAdapterFanSpeed = (CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterFanSpeed)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterFanSpeed));
                    result = cpuidsdk_fp_GetDisplayAdapterFanSpeed(this.objptr, _adapter_index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000518 RID: 1304 RVA: 0x000315E8 File Offset: 0x0002F7E8
        public int GetDisplayAdapterFanPWM(int _adapter_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2268794486u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterFanPWM cpuidsdk_fp_GetDisplayAdapterFanPWM = (CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterFanPWM)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterFanPWM));
                    result = cpuidsdk_fp_GetDisplayAdapterFanPWM(this.objptr, _adapter_index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000519 RID: 1305 RVA: 0x00031664 File Offset: 0x0002F864
        public bool GetDisplayAdapterMemoryType(int _adapter_index, ref int _type)
        {
            bool result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1781060689u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterMemoryType cpuidsdk_fp_GetDisplayAdapterMemoryType = (CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterMemoryType)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterMemoryType));
                    int num = cpuidsdk_fp_GetDisplayAdapterMemoryType(this.objptr, _adapter_index, ref _type);
                    bool flag2 = num == 1;
                    if (flag2)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        // Token: 0x0600051A RID: 1306 RVA: 0x000316EC File Offset: 0x0002F8EC
        public bool GetDisplayAdapterMemorySize(int _adapter_index, ref int _size)
        {
            bool result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2999018882u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterMemorySize cpuidsdk_fp_GetDisplayAdapterMemorySize = (CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterMemorySize)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterMemorySize));
                    int num = cpuidsdk_fp_GetDisplayAdapterMemorySize(this.objptr, _adapter_index, ref _size);
                    bool flag2 = num == 1;
                    if (flag2)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        // Token: 0x0600051B RID: 1307 RVA: 0x00031774 File Offset: 0x0002F974
        public bool GetDisplayAdapterMemoryBusWidth(int _adapter_index, ref int _bus_width)
        {
            bool result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2319520898u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterMemoryBusWidth cpuidsdk_fp_GetDisplayAdapterMemoryBusWidth = (CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterMemoryBusWidth)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterMemoryBusWidth));
                    int num = cpuidsdk_fp_GetDisplayAdapterMemoryBusWidth(this.objptr, _adapter_index, ref _bus_width);
                    bool flag2 = num == 1;
                    if (flag2)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        // Token: 0x0600051C RID: 1308 RVA: 0x000317FC File Offset: 0x0002F9FC
        public string GetDisplayDriverVersion()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(750279024u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDisplayDriverVersion cpuidsdk_fp_GetDisplayDriverVersion = (CPUIDSDK.CPUIDSDK_fp_GetDisplayDriverVersion)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDisplayDriverVersion));
                    IntPtr ptr = cpuidsdk_fp_GetDisplayDriverVersion(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x0600051D RID: 1309 RVA: 0x00031884 File Offset: 0x0002FA84
        public string GetDirectXVersion()
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(980841708u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDirectXVersion cpuidsdk_fp_GetDirectXVersion = (CPUIDSDK.CPUIDSDK_fp_GetDirectXVersion)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDirectXVersion));
                    IntPtr ptr = cpuidsdk_fp_GetDirectXVersion(this.objptr);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x0600051E RID: 1310 RVA: 0x0003190C File Offset: 0x0002FB0C
        public bool GetDisplayAdapterBusInfos(int _adapter_index, ref int _bus_type, ref int _multi_vpu)
        {
            bool result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3653874579u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterBusInfos cpuidsdk_fp_GetDisplayAdapterBusInfos = (CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterBusInfos)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDisplayAdapterBusInfos));
                    int num = cpuidsdk_fp_GetDisplayAdapterBusInfos(this.objptr, _adapter_index, ref _bus_type, ref _multi_vpu);
                    bool flag2 = num == 1;
                    if (flag2)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        // Token: 0x0600051F RID: 1311 RVA: 0x00031994 File Offset: 0x0002FB94
        public int GetNumberOfMonitors()
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(570835980u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetNumberOfMonitors cpuidsdk_fp_GetNumberOfMonitors = (CPUIDSDK.CPUIDSDK_fp_GetNumberOfMonitors)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetNumberOfMonitors));
                    result = cpuidsdk_fp_GetNumberOfMonitors(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000520 RID: 1312 RVA: 0x00031A0C File Offset: 0x0002FC0C
        public string GetMonitorName(int _monitor_index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1004959692u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMonitorName cpuidsdk_fp_GetMonitorName = (CPUIDSDK.CPUIDSDK_fp_GetMonitorName)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMonitorName));
                    IntPtr ptr = cpuidsdk_fp_GetMonitorName(this.objptr, _monitor_index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x06000521 RID: 1313 RVA: 0x00031A94 File Offset: 0x0002FC94
        public string GetMonitorVendor(int _monitor_index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2102327965u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMonitorVendor cpuidsdk_fp_GetMonitorVendor = (CPUIDSDK.CPUIDSDK_fp_GetMonitorVendor)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMonitorVendor));
                    IntPtr ptr = cpuidsdk_fp_GetMonitorVendor(this.objptr, _monitor_index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x06000522 RID: 1314 RVA: 0x00031B1C File Offset: 0x0002FD1C
        public string GetMonitorID(int _monitor_index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1562294845u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMonitorID cpuidsdk_fp_GetMonitorID = (CPUIDSDK.CPUIDSDK_fp_GetMonitorID)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMonitorID));
                    IntPtr ptr = cpuidsdk_fp_GetMonitorID(this.objptr, _monitor_index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x06000523 RID: 1315 RVA: 0x00031BA4 File Offset: 0x0002FDA4
        public string GetMonitorSerial(int _monitor_index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3314387739u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMonitorSerial cpuidsdk_fp_GetMonitorSerial = (CPUIDSDK.CPUIDSDK_fp_GetMonitorSerial)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMonitorSerial));
                    IntPtr ptr = cpuidsdk_fp_GetMonitorSerial(this.objptr, _monitor_index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x06000524 RID: 1316 RVA: 0x00031C2C File Offset: 0x0002FE2C
        public bool GetMonitorManufacturingDate(int _monitor_index, ref int _week, ref int _year)
        {
            bool result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2743682834u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMonitorManufacturingDate cpuidsdk_fp_GetMonitorManufacturingDate = (CPUIDSDK.CPUIDSDK_fp_GetMonitorManufacturingDate)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMonitorManufacturingDate));
                    int num = cpuidsdk_fp_GetMonitorManufacturingDate(this.objptr, _monitor_index, ref _week, ref _year);
                    bool flag2 = num == 1;
                    if (flag2)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        // Token: 0x06000525 RID: 1317 RVA: 0x00031CB4 File Offset: 0x0002FEB4
        public float GetMonitorSize(int _monitor_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3937260891u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMonitorSize cpuidsdk_fp_GetMonitorSize = (CPUIDSDK.CPUIDSDK_fp_GetMonitorSize)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMonitorSize));
                    result = cpuidsdk_fp_GetMonitorSize(this.objptr, _monitor_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000526 RID: 1318 RVA: 0x00031D30 File Offset: 0x0002FF30
        public bool GetMonitorResolution(int _monitor_index, ref int _width, ref int _height, ref int _frequency)
        {
            bool result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2874496682u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMonitorResolution cpuidsdk_fp_GetMonitorResolution = (CPUIDSDK.CPUIDSDK_fp_GetMonitorResolution)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMonitorResolution));
                    int num = cpuidsdk_fp_GetMonitorResolution(this.objptr, _monitor_index, ref _width, ref _height, ref _frequency);
                    bool flag2 = num == 1;
                    if (flag2)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        // Token: 0x06000527 RID: 1319 RVA: 0x00031DBC File Offset: 0x0002FFBC
        public int GetMonitorMaxPixelClock(int _monitor_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1222939081u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMonitorMaxPixelClock cpuidsdk_fp_GetMonitorMaxPixelClock = (CPUIDSDK.CPUIDSDK_fp_GetMonitorMaxPixelClock)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMonitorMaxPixelClock));
                    result = cpuidsdk_fp_GetMonitorMaxPixelClock(this.objptr, _monitor_index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000528 RID: 1320 RVA: 0x00031E38 File Offset: 0x00030038
        public float GetMonitorGamma(int _monitor_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2024862049u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetMonitorGamma cpuidsdk_fp_GetMonitorGamma = (CPUIDSDK.CPUIDSDK_fp_GetMonitorGamma)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetMonitorGamma));
                    result = cpuidsdk_fp_GetMonitorGamma(this.objptr, _monitor_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000529 RID: 1321 RVA: 0x00031EB4 File Offset: 0x000300B4
        public int GetNumberOfStorageDevice()
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3223552071u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetNumberOfStorageDevice cpuidsdk_fp_GetNumberOfStorageDevice = (CPUIDSDK.CPUIDSDK_fp_GetNumberOfStorageDevice)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetNumberOfStorageDevice));
                    result = cpuidsdk_fp_GetNumberOfStorageDevice(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x0600052A RID: 1322 RVA: 0x00031F2C File Offset: 0x0003012C
        public int GetStorageDriveNumber(int _index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3315567423u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetStorageDriveNumber cpuidsdk_fp_GetStorageDriveNumber = (CPUIDSDK.CPUIDSDK_fp_GetStorageDriveNumber)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetStorageDriveNumber));
                    result = cpuidsdk_fp_GetStorageDriveNumber(this.objptr, _index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x0600052B RID: 1323 RVA: 0x00031FA8 File Offset: 0x000301A8
        public string GetStorageDeviceName(int _index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2971755074u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceName cpuidsdk_fp_GetStorageDeviceName = (CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceName)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceName));
                    IntPtr ptr = cpuidsdk_fp_GetStorageDeviceName(this.objptr, _index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x0600052C RID: 1324 RVA: 0x00032030 File Offset: 0x00030230
        public string GetStorageDeviceRevision(int _index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(3013306166u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceRevision cpuidsdk_fp_GetStorageDeviceRevision = (CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceRevision)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceRevision));
                    IntPtr ptr = cpuidsdk_fp_GetStorageDeviceRevision(this.objptr, _index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x0600052D RID: 1325 RVA: 0x000320B8 File Offset: 0x000302B8
        public string GetStorageDeviceSerialNumber(int _index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2691383510u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceSerialNumber cpuidsdk_fp_GetStorageDeviceSerialNumber = (CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceSerialNumber)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceSerialNumber));
                    IntPtr ptr = cpuidsdk_fp_GetStorageDeviceSerialNumber(this.objptr, _index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x0600052E RID: 1326 RVA: 0x00032140 File Offset: 0x00030340
        public int GetStorageDeviceBusType(int _index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(350890452u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceBusType cpuidsdk_fp_GetStorageDeviceBusType = (CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceBusType)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceBusType));
                    result = cpuidsdk_fp_GetStorageDeviceBusType(this.objptr, _index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x0600052F RID: 1327 RVA: 0x000321BC File Offset: 0x000303BC
        public int GetStorageDeviceRotationSpeed(int _index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1230672565u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceRotationSpeed cpuidsdk_fp_GetStorageDeviceRotationSpeed = (CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceRotationSpeed)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceRotationSpeed));
                    result = cpuidsdk_fp_GetStorageDeviceRotationSpeed(this.objptr, _index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000530 RID: 1328 RVA: 0x00032238 File Offset: 0x00030438
        public uint GetStorageDeviceFeatureFlag(int _index)
        {
            uint result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1438690177u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceFeatureFlag cpuidsdk_fp_GetStorageDeviceFeatureFlag = (CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceFeatureFlag)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceFeatureFlag));
                    result = (uint)cpuidsdk_fp_GetStorageDeviceFeatureFlag(this.objptr, _index);
                }
                else
                {
                    result = (uint)CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = (uint)CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000531 RID: 1329 RVA: 0x000322B4 File Offset: 0x000304B4
        public int GetStorageDeviceNumberOfVolumes(int _index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2646817670u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceNumberOfVolumes cpuidsdk_fp_GetStorageDeviceNumberOfVolumes = (CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceNumberOfVolumes)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceNumberOfVolumes));
                    result = cpuidsdk_fp_GetStorageDeviceNumberOfVolumes(this.objptr, _index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000532 RID: 1330 RVA: 0x00032330 File Offset: 0x00030530
        public string GetStorageDeviceVolumeLetter(int _index, int _volume_index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2741716694u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceVolumeLetter cpuidsdk_fp_GetStorageDeviceVolumeLetter = (CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceVolumeLetter)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceVolumeLetter));
                    IntPtr ptr = cpuidsdk_fp_GetStorageDeviceVolumeLetter(this.objptr, _index, _volume_index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x06000533 RID: 1331 RVA: 0x000323B8 File Offset: 0x000305B8
        public float GetStorageDeviceVolumeTotalCapacity(int _index, int _volume_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(184817160u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceVolumeTotalCapacity cpuidsdk_fp_GetStorageDeviceVolumeTotalCapacity = (CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceVolumeTotalCapacity)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceVolumeTotalCapacity));
                    result = cpuidsdk_fp_GetStorageDeviceVolumeTotalCapacity(this.objptr, _index, _volume_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000534 RID: 1332 RVA: 0x00032434 File Offset: 0x00030634
        public float GetStorageDeviceVolumeAvailableCapacity(int _index, int _volume_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2509712174u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceVolumeAvailableCapacity cpuidsdk_fp_GetStorageDeviceVolumeAvailableCapacity = (CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceVolumeAvailableCapacity)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceVolumeAvailableCapacity));
                    result = cpuidsdk_fp_GetStorageDeviceVolumeAvailableCapacity(this.objptr, _index, _volume_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000535 RID: 1333 RVA: 0x000324B0 File Offset: 0x000306B0
        public bool GetStorageDeviceSmartAttribute(int _hdd_index, int _attrib_index, ref int _id, ref int _flags, ref int _value, ref int _worst, byte[] _data)
        {
            bool result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2696233322u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceSmartAttribute cpuidsdk_fp_GetStorageDeviceSmartAttribute = (CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceSmartAttribute)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetStorageDeviceSmartAttribute));
                    int num = cpuidsdk_fp_GetStorageDeviceSmartAttribute(this.objptr, _hdd_index, _attrib_index, ref _id, ref _flags, ref _value, ref _worst, _data);
                    bool flag2 = num == 1;
                    if (flag2)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        // Token: 0x06000536 RID: 1334 RVA: 0x00032540 File Offset: 0x00030740
        public int GetStorageDevicePowerOnHours(int _index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2389646558u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetStorageDevicePowerOnHours cpuidsdk_fp_GetStorageDevicePowerOnHours = (CPUIDSDK.CPUIDSDK_fp_GetStorageDevicePowerOnHours)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetStorageDevicePowerOnHours));
                    result = cpuidsdk_fp_GetStorageDevicePowerOnHours(this.objptr, _index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000537 RID: 1335 RVA: 0x000325BC File Offset: 0x000307BC
        public int GetStorageDevicePowerCycleCount(int _index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1762447897u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetStorageDevicePowerCycleCount cpuidsdk_fp_GetStorageDevicePowerCycleCount = (CPUIDSDK.CPUIDSDK_fp_GetStorageDevicePowerCycleCount)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetStorageDevicePowerCycleCount));
                    result = cpuidsdk_fp_GetStorageDevicePowerCycleCount(this.objptr, _index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000538 RID: 1336 RVA: 0x00032638 File Offset: 0x00030838
        public int GetNumberOfDevices()
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(856188432u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetNumberOfDevices cpuidsdk_fp_GetNumberOfDevices = (CPUIDSDK.CPUIDSDK_fp_GetNumberOfDevices)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetNumberOfDevices));
                    result = cpuidsdk_fp_GetNumberOfDevices(this.objptr);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x06000539 RID: 1337 RVA: 0x000326B0 File Offset: 0x000308B0
        public int GetDeviceClass(int _device_index)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2560831814u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDeviceClass cpuidsdk_fp_GetDeviceClass = (CPUIDSDK.CPUIDSDK_fp_GetDeviceClass)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDeviceClass));
                    result = cpuidsdk_fp_GetDeviceClass(this.objptr, _device_index);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x0600053A RID: 1338 RVA: 0x0003272C File Offset: 0x0003092C
        public string GetDeviceName(int _device_index)
        {
            string result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(2291863862u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetDeviceName cpuidsdk_fp_GetDeviceName = (CPUIDSDK.CPUIDSDK_fp_GetDeviceName)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetDeviceName));
                    IntPtr ptr = cpuidsdk_fp_GetDeviceName(this.objptr, _device_index);
                    string text = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeBSTR(ptr);
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x0600053B RID: 1339 RVA: 0x000327B4 File Offset: 0x000309B4
        public int GetNumberOfSensors(int _device_index, int _sensor_class)
        {
            int result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1246663837u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetNumberOfSensors cpuidsdk_fp_GetNumberOfSensors = (CPUIDSDK.CPUIDSDK_fp_GetNumberOfSensors)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetNumberOfSensors));
                    result = cpuidsdk_fp_GetNumberOfSensors(this.objptr, _device_index, _sensor_class);
                }
                else
                {
                    result = CPUIDSDK.I_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.I_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x0600053C RID: 1340 RVA: 0x00032830 File Offset: 0x00030A30
        public bool GetSensorInfos(int _device_index, int _sensor_index, int _sensor_class, ref int _sensor_id, ref string _szName, ref int _raw_value, ref float _value, ref float _min_value, ref float _max_value)
        {
            bool result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(745560288u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    IntPtr zero = IntPtr.Zero;
                    CPUIDSDK.CPUIDSDK_fp_GetSensorInfos cpuidsdk_fp_GetSensorInfos = (CPUIDSDK.CPUIDSDK_fp_GetSensorInfos)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSensorInfos));
                    int num = cpuidsdk_fp_GetSensorInfos(this.objptr, _device_index, _sensor_index, _sensor_class, ref _sensor_id, ref zero, ref _raw_value, ref _value, ref _min_value, ref _max_value);
                    bool flag2 = num == 1;
                    if (flag2)
                    {
                        _szName = Marshal.PtrToStringAnsi(zero);
                        Marshal.FreeBSTR(zero);
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        // Token: 0x0600053D RID: 1341 RVA: 0x000328E0 File Offset: 0x00030AE0
        public void SensorClearMinMax(int _device_index, int _sensor_index, int _sensor_class, ref int _sensor_id, ref string _szName, ref int _raw_value, ref float _value, ref float _min_value, ref float _max_value)
        {
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(346171716u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    IntPtr zero = IntPtr.Zero;
                    CPUIDSDK.CPUIDSDK_fp_SensorClearMinMax cpuidsdk_fp_SensorClearMinMax = (CPUIDSDK.CPUIDSDK_fp_SensorClearMinMax)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_SensorClearMinMax));
                    cpuidsdk_fp_SensorClearMinMax(this.objptr, _device_index, _sensor_index, _sensor_class);
                }
            }
            catch
            {
            }
        }

        // Token: 0x0600053E RID: 1342 RVA: 0x00032958 File Offset: 0x00030B58
        public float GetSensorTypeValue(int _sensor_type, ref int _device_index, ref int _sensor_index)
        {
            float result;
            try
            {
                IntPtr intPtr = this.CPUIDSDK_fp_QueryInterface(1968892597u);
                bool flag = intPtr != IntPtr.Zero;
                if (flag)
                {
                    CPUIDSDK.CPUIDSDK_fp_GetSensorTypeValue cpuidsdk_fp_GetSensorTypeValue = (CPUIDSDK.CPUIDSDK_fp_GetSensorTypeValue)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CPUIDSDK.CPUIDSDK_fp_GetSensorTypeValue));
                    result = cpuidsdk_fp_GetSensorTypeValue(this.objptr, _sensor_type, ref _device_index, ref _sensor_index);
                }
                else
                {
                    result = CPUIDSDK.F_UNDEFINED_VALUE;
                }
            }
            catch
            {
                result = CPUIDSDK.F_UNDEFINED_VALUE;
            }
            return result;
        }

        // Token: 0x0400030F RID: 783
        public const uint CPUIDSDK_ERROR_NO_ERROR = 0u;

        // Token: 0x04000310 RID: 784
        public const uint CPUIDSDK_ERROR_EVALUATION = 1u;

        // Token: 0x04000311 RID: 785
        public const uint CPUIDSDK_ERROR_DRIVER = 2u;

        // Token: 0x04000312 RID: 786
        public const uint CPUIDSDK_ERROR_VM_RUNNING = 4u;

        // Token: 0x04000313 RID: 787
        public const uint CPUIDSDK_ERROR_LOCKED = 8u;

        // Token: 0x04000314 RID: 788
        public const uint CPUIDSDK_ERROR_INVALID_DLL = 16u;

        // Token: 0x04000315 RID: 789
        public const uint CPUIDSDK_EXT_ERROR_EVAL_1 = 1u;

        // Token: 0x04000316 RID: 790
        public const uint CPUIDSDK_EXT_ERROR_EVAL_2 = 2u;

        // Token: 0x04000317 RID: 791
        public const uint CPUIDSDK_CONFIG_USE_SOFTWARE = 2u;

        // Token: 0x04000318 RID: 792
        public const uint CPUIDSDK_CONFIG_USE_DMI = 4u;

        // Token: 0x04000319 RID: 793
        public const uint CPUIDSDK_CONFIG_USE_PCI = 8u;

        // Token: 0x0400031A RID: 794
        public const uint CPUIDSDK_CONFIG_USE_ACPI = 16u;

        // Token: 0x0400031B RID: 795
        public const uint CPUIDSDK_CONFIG_USE_CHIPSET = 32u;

        // Token: 0x0400031C RID: 796
        public const uint CPUIDSDK_CONFIG_USE_SMBUS = 64u;

        // Token: 0x0400031D RID: 797
        public const uint CPUIDSDK_CONFIG_USE_SPD = 128u;

        // Token: 0x0400031E RID: 798
        public const uint CPUIDSDK_CONFIG_USE_STORAGE = 256u;

        // Token: 0x0400031F RID: 799
        public const uint CPUIDSDK_CONFIG_USE_GRAPHICS = 512u;

        // Token: 0x04000320 RID: 800
        public const uint CPUIDSDK_CONFIG_USE_HWMONITORING = 1024u;

        // Token: 0x04000321 RID: 801
        public const uint CPUIDSDK_CONFIG_USE_PROCESSOR = 2048u;

        // Token: 0x04000322 RID: 802
        public const uint CPUIDSDK_CONFIG_USE_DISPLAY_API = 4096u;

        // Token: 0x04000323 RID: 803
        public const uint CPUIDSDK_CONFIG_USE_ACPI_TIMER = 16384u;

        // Token: 0x04000324 RID: 804
        public const uint CPUIDSDK_CONFIG_UPDATE_PROCESSOR = 65536u;

        // Token: 0x04000325 RID: 805
        public const uint CPUIDSDK_CONFIG_UPDATE_GRAPHICS = 131072u;

        // Token: 0x04000326 RID: 806
        public const uint CPUIDSDK_CONFIG_UPDATE_STORAGE = 262144u;

        // Token: 0x04000327 RID: 807
        public const uint CPUIDSDK_CONFIG_UPDATE_LPCIO = 524288u;

        // Token: 0x04000328 RID: 808
        public const uint CPUIDSDK_CONFIG_UPDATE_DRAM = 1048576u;

        // Token: 0x04000329 RID: 809
        public const uint CPUIDSDK_CONFIG_CHECK_VM = 16777216u;

        // Token: 0x0400032A RID: 810
        public const uint CPUIDSDK_CONFIG_WAKEUP_HDD = 33554432u;

        // Token: 0x0400032B RID: 811
        public const uint CPUIDSDK_CONFIG_SERVER_SAFE = 2147483648u;

        // Token: 0x0400032C RID: 812
        public const uint CPUIDSDK_CONFIG_USE_EVERYTHING = 2147483647u;

        // Token: 0x0400032D RID: 813
        public static int I_UNDEFINED_VALUE = -1;

        // Token: 0x0400032E RID: 814
        public static float F_UNDEFINED_VALUE = -1f;

        // Token: 0x0400032F RID: 815
        public static uint MAX_INTEGER = uint.MaxValue;

        // Token: 0x04000330 RID: 816
        public static float MAX_FLOAT = CPUIDSDK.MAX_INTEGER;

        // Token: 0x04000331 RID: 817
        public const uint CLASS_DEVICE_UNKNOWN = 0u;

        // Token: 0x04000332 RID: 818
        public const uint CLASS_DEVICE_PCI = 1u;

        // Token: 0x04000333 RID: 819
        public const uint CLASS_DEVICE_SMBUS = 2u;

        // Token: 0x04000334 RID: 820
        public const uint CLASS_DEVICE_PROCESSOR = 4u;

        // Token: 0x04000335 RID: 821
        public const uint CLASS_DEVICE_LPCIO = 8u;

        // Token: 0x04000336 RID: 822
        public const uint CLASS_DEVICE_DRIVE = 16u;

        // Token: 0x04000337 RID: 823
        public const uint CLASS_DEVICE_DISPLAY_ADAPTER = 32u;

        // Token: 0x04000338 RID: 824
        public const uint CLASS_DEVICE_HID = 64u;

        // Token: 0x04000339 RID: 825
        public const uint CLASS_DEVICE_BATTERY = 128u;

        // Token: 0x0400033A RID: 826
        public const uint CLASS_DEVICE_EVBOT = 256u;

        // Token: 0x0400033B RID: 827
        public const uint CLASS_DEVICE_NETWORK = 512u;

        // Token: 0x0400033C RID: 828
        public const uint CLASS_DEVICE_MAINBOARD = 1024u;

        // Token: 0x0400033D RID: 829
        public const uint CLASS_DEVICE_MEMORY_MODULE = 2048u;

        // Token: 0x0400033E RID: 830
        public const uint CLASS_DEVICE_PSU = 4096u;

        // Token: 0x0400033F RID: 831
        public const uint CLASS_DEVICE_TYPE_MASK = 2147483647u;

        // Token: 0x04000340 RID: 832
        public const uint CLASS_DEVICE_COMPOSITE = 2147483648u;

        // Token: 0x04000341 RID: 833
        public const uint CPU_MANUFACTURER_MASK = 4278190080u;

        // Token: 0x04000342 RID: 834
        public const uint CPU_FAMILY_MASK = 4294967040u;

        // Token: 0x04000343 RID: 835
        public const uint CPU_MODEL_MASK = 4294967295u;

        // Token: 0x04000344 RID: 836
        public const uint CPU_UNKNOWN = 0u;

        // Token: 0x04000345 RID: 837
        public const uint CPU_INTEL = 16777216u;

        // Token: 0x04000346 RID: 838
        public const uint CPU_AMD = 33554432u;

        // Token: 0x04000347 RID: 839
        public const uint CPU_CYRIX = 67108864u;

        // Token: 0x04000348 RID: 840
        public const uint CPU_VIA = 134217728u;

        // Token: 0x04000349 RID: 841
        public const uint CPU_TRANSMETA = 268435456u;

        // Token: 0x0400034A RID: 842
        public const uint CPU_DMP = 536870912u;

        // Token: 0x0400034B RID: 843
        public const uint CPU_UMC = 1073741824u;

        // Token: 0x0400034C RID: 844
        public const uint CPU_INTEL_386 = 16777472u;

        // Token: 0x0400034D RID: 845
        public const uint CPU_INTEL_486 = 16777728u;

        // Token: 0x0400034E RID: 846
        public const uint CPU_INTEL_P5 = 16778240u;

        // Token: 0x0400034F RID: 847
        public const uint CPU_INTEL_P6 = 16779264u;

        // Token: 0x04000350 RID: 848
        public const uint CPU_INTEL_NETBURST = 16781312u;

        // Token: 0x04000351 RID: 849
        public const uint CPU_INTEL_MOBILE = 16785408u;

        // Token: 0x04000352 RID: 850
        public const uint CPU_INTEL_CORE = 16793600u;

        // Token: 0x04000353 RID: 851
        public const uint CPU_INTEL_CORE_2 = 16809984u;

        // Token: 0x04000354 RID: 852
        public const uint CPU_INTEL_BONNELL = 16842752u;

        // Token: 0x04000355 RID: 853
        public const uint CPU_INTEL_SALTWELL = 16843008u;

        // Token: 0x04000356 RID: 854
        public const uint CPU_INTEL_SILVERMONT = 16843264u;

        // Token: 0x04000357 RID: 855
        public const uint CPU_INTEL_GOLDMONT = 16843776u;

        // Token: 0x04000358 RID: 856
        public const uint CPU_INTEL_NEHALEM = 16908288u;

        // Token: 0x04000359 RID: 857
        public const uint CPU_INTEL_SANDY_BRIDGE = 16908544u;

        // Token: 0x0400035A RID: 858
        public const uint CPU_INTEL_HASWELL = 16908800u;

        // Token: 0x0400035B RID: 859
        public const uint CPU_INTEL_SKYLAKE = 17039360u;

        // Token: 0x0400035C RID: 860
        public const uint CPU_INTEL_ITANIUM = 17825792u;

        // Token: 0x0400035D RID: 861
        public const uint CPU_INTEL_ITANIUM_2 = 17826048u;

        // Token: 0x0400035E RID: 862
        public const uint CPU_INTEL_MIC = 18874368u;

        // Token: 0x0400035F RID: 863
        public const uint CPU_PENTIUM = 16778241u;

        // Token: 0x04000360 RID: 864
        public const uint CPU_PENTIUM_MMX = 16778242u;

        // Token: 0x04000361 RID: 865
        public const uint CPU_PENTIUM_PRO = 16779265u;

        // Token: 0x04000362 RID: 866
        public const uint CPU_PENTIUM_2 = 16779266u;

        // Token: 0x04000363 RID: 867
        public const uint CPU_PENTIUM_2_M = 16779267u;

        // Token: 0x04000364 RID: 868
        public const uint CPU_CELERON_P2 = 16779268u;

        // Token: 0x04000365 RID: 869
        public const uint CPU_XEON_P2 = 16779269u;

        // Token: 0x04000366 RID: 870
        public const uint CPU_PENTIUM_3 = 16779270u;

        // Token: 0x04000367 RID: 871
        public const uint CPU_PENTIUM_3_M = 16779271u;

        // Token: 0x04000368 RID: 872
        public const uint CPU_PENTIUM_3_S = 16779272u;

        // Token: 0x04000369 RID: 873
        public const uint CPU_CELERON_P3 = 16779273u;

        // Token: 0x0400036A RID: 874
        public const uint CPU_XEON_P3 = 16779274u;

        // Token: 0x0400036B RID: 875
        public const uint CPU_PENTIUM_4 = 16781313u;

        // Token: 0x0400036C RID: 876
        public const uint CPU_PENTIUM_4_M = 16781314u;

        // Token: 0x0400036D RID: 877
        public const uint CPU_PENTIUM_4_HT = 16781315u;

        // Token: 0x0400036E RID: 878
        public const uint CPU_PENTIUM_4_EE = 16781316u;

        // Token: 0x0400036F RID: 879
        public const uint CPU_CELERON_P4 = 16781317u;

        // Token: 0x04000370 RID: 880
        public const uint CPU_CELERON_D = 16781318u;

        // Token: 0x04000371 RID: 881
        public const uint CPU_XEON_P4 = 16781319u;

        // Token: 0x04000372 RID: 882
        public const uint CPU_PENTIUM_D = 16781320u;

        // Token: 0x04000373 RID: 883
        public const uint CPU_PENTIUM_XE = 16781321u;

        // Token: 0x04000374 RID: 884
        public const uint CPU_PENTIUM_M = 16785409u;

        // Token: 0x04000375 RID: 885
        public const uint CPU_CELERON_M = 16785410u;

        // Token: 0x04000376 RID: 886
        public const uint CPU_CORE_SOLO = 16793601u;

        // Token: 0x04000377 RID: 887
        public const uint CPU_CORE_DUO = 16793602u;

        // Token: 0x04000378 RID: 888
        public const uint CPU_CORE_CELERON_M = 16793603u;

        // Token: 0x04000379 RID: 889
        public const uint CPU_CORE_CELERON = 16793604u;

        // Token: 0x0400037A RID: 890
        public const uint CPU_CORE_2_DUO = 16809985u;

        // Token: 0x0400037B RID: 891
        public const uint CPU_CORE_2_EE = 16809986u;

        // Token: 0x0400037C RID: 892
        public const uint CPU_CORE_2_XEON = 16809987u;

        // Token: 0x0400037D RID: 893
        public const uint CPU_CORE_2_CELERON = 16809988u;

        // Token: 0x0400037E RID: 894
        public const uint CPU_CORE_2_QUAD = 16809989u;

        // Token: 0x0400037F RID: 895
        public const uint CPU_CORE_2_PENTIUM = 16809990u;

        // Token: 0x04000380 RID: 896
        public const uint CPU_CORE_2_CELERON_DC = 16809991u;

        // Token: 0x04000381 RID: 897
        public const uint CPU_CORE_2_SOLO = 16809992u;

        // Token: 0x04000382 RID: 898
        public const uint CPU_BONNELL_ATOM = 16842753u;

        // Token: 0x04000383 RID: 899
        public const uint CPU_SALTWELL_ATOM = 16843009u;

        // Token: 0x04000384 RID: 900
        public const uint CPU_SILVERMONT_ATOM = 16843265u;

        // Token: 0x04000385 RID: 901
        public const uint CPU_SILVERMONT_CELERON = 16843266u;

        // Token: 0x04000386 RID: 902
        public const uint CPU_SILVERMONT_PENTIUM = 16843267u;

        // Token: 0x04000387 RID: 903
        public const uint CPU_SILVERMONT_ATOM_X7 = 16843268u;

        // Token: 0x04000388 RID: 904
        public const uint CPU_SILVERMONT_ATOM_X5 = 16843269u;

        // Token: 0x04000389 RID: 905
        public const uint CPU_SILVERMONT_ATOM_X3 = 16843270u;

        // Token: 0x0400038A RID: 906
        public const uint CPU_GOLDMONT_ATOM = 16843777u;

        // Token: 0x0400038B RID: 907
        public const uint CPU_GOLDMONT_CELERON = 16843778u;

        // Token: 0x0400038C RID: 908
        public const uint CPU_GOLDMONT_PENTIUM = 16843779u;

        // Token: 0x0400038D RID: 909
        public const uint CPU_NEHALEM_CORE_I7 = 16908289u;

        // Token: 0x0400038E RID: 910
        public const uint CPU_NEHALEM_CORE_I7E = 16908290u;

        // Token: 0x0400038F RID: 911
        public const uint CPU_NEHALEM_XEON = 16908291u;

        // Token: 0x04000390 RID: 912
        public const uint CPU_NEHALEM_CORE_I3 = 16908292u;

        // Token: 0x04000391 RID: 913
        public const uint CPU_NEHALEM_CORE_I5 = 16908293u;

        // Token: 0x04000392 RID: 914
        public const uint CPU_NEHALEM_PENTIUM = 16908295u;

        // Token: 0x04000393 RID: 915
        public const uint CPU_NEHALEM_CELERON = 16908296u;

        // Token: 0x04000394 RID: 916
        public const uint CPU_SANDY_BRIDGE_CORE_I7 = 16908545u;

        // Token: 0x04000395 RID: 917
        public const uint CPU_SANDY_BRIDGE_CORE_I7E = 16908546u;

        // Token: 0x04000396 RID: 918
        public const uint CPU_SANDY_BRIDGE_XEON = 16908547u;

        // Token: 0x04000397 RID: 919
        public const uint CPU_SANDY_BRIDGE_CORE_I3 = 16908548u;

        // Token: 0x04000398 RID: 920
        public const uint CPU_SANDY_BRIDGE_CORE_I5 = 16908549u;

        // Token: 0x04000399 RID: 921
        public const uint CPU_SANDY_BRIDGE_PENTIUM = 16908551u;

        // Token: 0x0400039A RID: 922
        public const uint CPU_SANDY_BRIDGE_CELERON = 16908552u;

        // Token: 0x0400039B RID: 923
        public const uint CPU_HASWELL_CORE_I7 = 16908801u;

        // Token: 0x0400039C RID: 924
        public const uint CPU_HASWELL_CORE_I7E = 16908802u;

        // Token: 0x0400039D RID: 925
        public const uint CPU_HASWELL_XEON = 16908803u;

        // Token: 0x0400039E RID: 926
        public const uint CPU_HASWELL_CORE_I3 = 16908804u;

        // Token: 0x0400039F RID: 927
        public const uint CPU_HASWELL_CORE_I5 = 16908805u;

        // Token: 0x040003A0 RID: 928
        public const uint CPU_HASWELL_PENTIUM = 16908807u;

        // Token: 0x040003A1 RID: 929
        public const uint CPU_HASWELL_CELERON = 16908808u;

        // Token: 0x040003A2 RID: 930
        public const uint CPU_HASWELL_CORE_M = 16908809u;

        // Token: 0x040003A3 RID: 931
        public const uint CPU_SKYLAKE_XEON = 17039361u;

        // Token: 0x040003A4 RID: 932
        public const uint CPU_SKYLAKE_CORE_I7 = 17039362u;

        // Token: 0x040003A5 RID: 933
        public const uint CPU_SKYLAKE_CORE_I5 = 17039363u;

        // Token: 0x040003A6 RID: 934
        public const uint CPU_SKYLAKE_CORE_I3 = 17039364u;

        // Token: 0x040003A7 RID: 935
        public const uint CPU_SKYLAKE_PENTIUM = 17039365u;

        // Token: 0x040003A8 RID: 936
        public const uint CPU_SKYLAKE_CELERON = 17039366u;

        // Token: 0x040003A9 RID: 937
        public const uint CPU_SKYLAKE_CORE_M7 = 17039367u;

        // Token: 0x040003AA RID: 938
        public const uint CPU_SKYLAKE_CORE_M5 = 17039368u;

        // Token: 0x040003AB RID: 939
        public const uint CPU_SKYLAKE_CORE_M3 = 17039369u;

        // Token: 0x040003AC RID: 940
        public const uint CPU_SKYLAKE_CORE_I9EX = 17039370u;

        // Token: 0x040003AD RID: 941
        public const uint CPU_SKYLAKE_CORE_I9X = 17039371u;

        // Token: 0x040003AE RID: 942
        public const uint CPU_SKYLAKE_CORE_I7X = 17039372u;

        // Token: 0x040003AF RID: 943
        public const uint CPU_SKYLAKE_CORE_I5X = 17039373u;

        // Token: 0x040003B0 RID: 944
        public const uint CPU_SKYLAKE_XEON_BRONZE = 17039374u;

        // Token: 0x040003B1 RID: 945
        public const uint CPU_SKYLAKE_XEON_SILVER = 17039375u;

        // Token: 0x040003B2 RID: 946
        public const uint CPU_SKYLAKE_XEON_GOLD = 17039376u;

        // Token: 0x040003B3 RID: 947
        public const uint CPU_SKYLAKE_XEON_PLATINIUM = 17039377u;

        // Token: 0x040003B4 RID: 948
        public const uint CPU_SKYLAKE_PENTIUM_GOLD = 17039378u;

        // Token: 0x040003B5 RID: 949
        public const uint CPU_AMD_386 = 33554688u;

        // Token: 0x040003B6 RID: 950
        public const uint CPU_AMD_486 = 33554944u;

        // Token: 0x040003B7 RID: 951
        public const uint CPU_AMD_K5 = 33555456u;

        // Token: 0x040003B8 RID: 952
        public const uint CPU_AMD_K6 = 33556480u;

        // Token: 0x040003B9 RID: 953
        public const uint CPU_AMD_K7 = 33558528u;

        // Token: 0x040003BA RID: 954
        public const uint CPU_AMD_K8 = 33562624u;

        // Token: 0x040003BB RID: 955
        public const uint CPU_AMD_K10 = 33570816u;

        // Token: 0x040003BC RID: 956
        public const uint CPU_AMD_K12 = 33619968u;

        // Token: 0x040003BD RID: 957
        public const uint CPU_AMD_K14 = 33685504u;

        // Token: 0x040003BE RID: 958
        public const uint CPU_AMD_K15 = 33816576u;

        // Token: 0x040003BF RID: 959
        public const uint CPU_AMD_K16 = 34078720u;

        // Token: 0x040003C0 RID: 960
        public const uint CPU_AMD_K17 = 34603008u;

        // Token: 0x040003C1 RID: 961
        public const uint CPU_K5 = 33555457u;

        // Token: 0x040003C2 RID: 962
        public const uint CPU_K5_GEODE = 33555458u;

        // Token: 0x040003C3 RID: 963
        public const uint CPU_K6 = 33556481u;

        // Token: 0x040003C4 RID: 964
        public const uint CPU_K6_2 = 33556482u;

        // Token: 0x040003C5 RID: 965
        public const uint CPU_K6_3 = 33556483u;

        // Token: 0x040003C6 RID: 966
        public const uint CPU_K7_ATHLON = 33558529u;

        // Token: 0x040003C7 RID: 967
        public const uint CPU_K7_ATHLON_XP = 33558530u;

        // Token: 0x040003C8 RID: 968
        public const uint CPU_K7_ATHLON_MP = 33558531u;

        // Token: 0x040003C9 RID: 969
        public const uint CPU_K7_DURON = 33558532u;

        // Token: 0x040003CA RID: 970
        public const uint CPU_K7_SEMPRON = 33558533u;

        // Token: 0x040003CB RID: 971
        public const uint CPU_K7_SEMPRON_M = 33558534u;

        // Token: 0x040003CC RID: 972
        public const uint CPU_K8_ATHLON_64 = 33562625u;

        // Token: 0x040003CD RID: 973
        public const uint CPU_K8_ATHLON_64_M = 33562626u;

        // Token: 0x040003CE RID: 974
        public const uint CPU_K8_ATHLON_64_FX = 33562627u;

        // Token: 0x040003CF RID: 975
        public const uint CPU_K8_OPTERON = 33562628u;

        // Token: 0x040003D0 RID: 976
        public const uint CPU_K8_TURION_64 = 33562629u;

        // Token: 0x040003D1 RID: 977
        public const uint CPU_K8_SEMPRON = 33562630u;

        // Token: 0x040003D2 RID: 978
        public const uint CPU_K8_SEMPRON_M = 33562631u;

        // Token: 0x040003D3 RID: 979
        public const uint CPU_K8_ATHLON_64_X2 = 33562632u;

        // Token: 0x040003D4 RID: 980
        public const uint CPU_K8_TURION_64_X2 = 33562633u;

        // Token: 0x040003D5 RID: 981
        public const uint CPU_K8_ATHLON_NEO = 33562634u;

        // Token: 0x040003D6 RID: 982
        public const uint CPU_K10_PHENOM = 33570817u;

        // Token: 0x040003D7 RID: 983
        public const uint CPU_K10_PHENOM_X3 = 33570818u;

        // Token: 0x040003D8 RID: 984
        public const uint CPU_K10_PHENOM_FX = 33570819u;

        // Token: 0x040003D9 RID: 985
        public const uint CPU_K10_OPTERON = 33570820u;

        // Token: 0x040003DA RID: 986
        public const uint CPU_K10_TURION_64 = 33570821u;

        // Token: 0x040003DB RID: 987
        public const uint CPU_K10_TURION_64_ULTRA = 33570822u;

        // Token: 0x040003DC RID: 988
        public const uint CPU_K10_ATHLON_64 = 33570823u;

        // Token: 0x040003DD RID: 989
        public const uint CPU_K10_SEMPRON = 33570824u;

        // Token: 0x040003DE RID: 990
        public const uint CPU_K10_ATHLON_2 = 33570833u;

        // Token: 0x040003DF RID: 991
        public const uint CPU_K10_ATHLON_2_X2 = 33570827u;

        // Token: 0x040003E0 RID: 992
        public const uint CPU_K10_ATHLON_2_X3 = 33570829u;

        // Token: 0x040003E1 RID: 993
        public const uint CPU_K10_ATHLON_2_X4 = 33570828u;

        // Token: 0x040003E2 RID: 994
        public const uint CPU_K10_PHENOM_II = 33570825u;

        // Token: 0x040003E3 RID: 995
        public const uint CPU_K10_PHENOM_II_X2 = 33570826u;

        // Token: 0x040003E4 RID: 996
        public const uint CPU_K10_PHENOM_II_X3 = 33570830u;

        // Token: 0x040003E5 RID: 997
        public const uint CPU_K10_PHENOM_II_X4 = 33570831u;

        // Token: 0x040003E6 RID: 998
        public const uint CPU_K10_PHENOM_II_X6 = 33570832u;

        // Token: 0x040003E7 RID: 999
        public const uint CPU_K15_FXB = 33816577u;

        // Token: 0x040003E8 RID: 1000
        public const uint CPU_K15_OPTERON = 33816578u;

        // Token: 0x040003E9 RID: 1001
        public const uint CPU_K15_A10T = 33816579u;

        // Token: 0x040003EA RID: 1002
        public const uint CPU_K15_A8T = 33816580u;

        // Token: 0x040003EB RID: 1003
        public const uint CPU_K15_A6T = 33816581u;

        // Token: 0x040003EC RID: 1004
        public const uint CPU_K15_A4T = 33816582u;

        // Token: 0x040003ED RID: 1005
        public const uint CPU_K15_ATHLON_X4 = 33816583u;

        // Token: 0x040003EE RID: 1006
        public const uint CPU_K15_FXV = 33816584u;

        // Token: 0x040003EF RID: 1007
        public const uint CPU_K15_A10R = 33816585u;

        // Token: 0x040003F0 RID: 1008
        public const uint CPU_K15_A8R = 33816586u;

        // Token: 0x040003F1 RID: 1009
        public const uint CPU_K15_A6R = 33816587u;

        // Token: 0x040003F2 RID: 1010
        public const uint CPU_K15_A4R = 33816588u;

        // Token: 0x040003F3 RID: 1011
        public const uint CPU_K15_SEMPRON = 33816589u;

        // Token: 0x040003F4 RID: 1012
        public const uint CPU_K15_ATHLON_X2 = 33816590u;

        // Token: 0x040003F5 RID: 1013
        public const uint CPU_K15_FXC = 33816591u;

        // Token: 0x040003F6 RID: 1014
        public const uint CPU_K15_A10C = 33816592u;

        // Token: 0x040003F7 RID: 1015
        public const uint CPU_K15_A8C = 33816593u;

        // Token: 0x040003F8 RID: 1016
        public const uint CPU_K15_A6C = 33816594u;

        // Token: 0x040003F9 RID: 1017
        public const uint CPU_K15_A4C = 33816595u;

        // Token: 0x040003FA RID: 1018
        public const uint CPU_K15_A12 = 33816596u;

        // Token: 0x040003FB RID: 1019
        public const uint CPU_K15_RX = 33816597u;

        // Token: 0x040003FC RID: 1020
        public const uint CPU_K15_GX = 33816598u;

        // Token: 0x040003FD RID: 1021
        public const uint CPU_K15_A9 = 33816599u;

        // Token: 0x040003FE RID: 1022
        public const uint CPU_K15_E2 = 33816600u;

        // Token: 0x040003FF RID: 1023
        public const uint CPU_K16_A6 = 34078721u;

        // Token: 0x04000400 RID: 1024
        public const uint CPU_K16_A4 = 34078722u;

        // Token: 0x04000401 RID: 1025
        public const uint CPU_K16_OPTERON = 34078725u;

        // Token: 0x04000402 RID: 1026
        public const uint CPU_K16_ATHLON = 34078726u;

        // Token: 0x04000403 RID: 1027
        public const uint CPU_K16_SEMPRON = 34078727u;

        // Token: 0x04000404 RID: 1028
        public const uint CPU_K16_E1 = 34078728u;

        // Token: 0x04000405 RID: 1029
        public const uint CPU_K16_E2 = 34078729u;

        // Token: 0x04000406 RID: 1030
        public const uint CPU_K16_A8 = 34078730u;

        // Token: 0x04000407 RID: 1031
        public const uint CPU_K16_A10 = 34078731u;

        // Token: 0x04000408 RID: 1032
        public const uint CPU_K16_GX = 34078732u;

        // Token: 0x04000409 RID: 1033
        public const uint CPU_RYZEN = 34603009u;

        // Token: 0x0400040A RID: 1034
        public const uint CPU_RYZEN_7 = 34603010u;

        // Token: 0x0400040B RID: 1035
        public const uint CPU_RYZEN_5 = 34603011u;

        // Token: 0x0400040C RID: 1036
        public const uint CPU_RYZEN_3 = 34603012u;

        // Token: 0x0400040D RID: 1037
        public const uint CPU_RYZEN_TR = 34603013u;

        // Token: 0x0400040E RID: 1038
        public const uint CPU_RYZEN_EPYC = 34603014u;

        // Token: 0x0400040F RID: 1039
        public const uint CPU_RYZEN_M = 34603015u;

        // Token: 0x04000410 RID: 1040
        public const uint CPU_RYZEN_7_M = 34603016u;

        // Token: 0x04000411 RID: 1041
        public const uint CPU_RYZEN_5_M = 34603017u;

        // Token: 0x04000412 RID: 1042
        public const uint CPU_RYZEN_3_M = 34603018u;

        // Token: 0x04000413 RID: 1043
        public const uint CPU_RYZEN_ATHLON = 34603019u;

        // Token: 0x04000414 RID: 1044
        public const uint CPU_CX486 = 67109888u;

        // Token: 0x04000415 RID: 1045
        public const uint CPU_CX5X86 = 67110144u;

        // Token: 0x04000416 RID: 1046
        public const uint CPU_CX6X86 = 67110400u;

        // Token: 0x04000417 RID: 1047
        public const uint CPU_VIA_WINCHIP = 134218752u;

        // Token: 0x04000418 RID: 1048
        public const uint CPU_VIA_C3 = 134219776u;

        // Token: 0x04000419 RID: 1049
        public const uint CPU_VIA_C7 = 134221824u;

        // Token: 0x0400041A RID: 1050
        public const uint CPU_VIA_NANO = 134225920u;

        // Token: 0x0400041B RID: 1051
        public const uint CPU_C3 = 134219777u;

        // Token: 0x0400041C RID: 1052
        public const uint CPU_C7 = 134221825u;

        // Token: 0x0400041D RID: 1053
        public const uint CPU_C7_M = 134221826u;

        // Token: 0x0400041E RID: 1054
        public const uint CPU_EDEN = 134221827u;

        // Token: 0x0400041F RID: 1055
        public const uint CPU_C7_D = 134221828u;

        // Token: 0x04000420 RID: 1056
        public const uint CPU_NANO_X2 = 134225921u;

        // Token: 0x04000421 RID: 1057
        public const uint CPU_EDEN_X2 = 134225922u;

        // Token: 0x04000422 RID: 1058
        public const uint CPU_NANO_X3 = 134225923u;

        // Token: 0x04000423 RID: 1059
        public const uint CPU_EDEN_X4 = 134225924u;

        // Token: 0x04000424 RID: 1060
        public const uint CPU_QUADCORE = 134225925u;

        // Token: 0x04000425 RID: 1061
        public const uint CPU_CX6X86L = 67110401u;

        // Token: 0x04000426 RID: 1062
        public const uint CPU_MEDIAGX = 67110402u;

        // Token: 0x04000427 RID: 1063
        public const uint CPU_CX6X86MX = 67110403u;

        // Token: 0x04000428 RID: 1064
        public const uint CPU_MII = 67110404u;

        // Token: 0x04000429 RID: 1065
        public const uint CPU_CRUSOE = 268435457u;

        // Token: 0x0400042A RID: 1066
        public const uint CPU_EFFICEON = 268435458u;

        // Token: 0x0400042B RID: 1067
        public const uint CPU_VORTEX86_SX = 536870913u;

        // Token: 0x0400042C RID: 1068
        public const uint CPU_VORTEX86_EX = 536870914u;

        // Token: 0x0400042D RID: 1069
        public const uint CPU_VORTEX86_DX = 536870915u;

        // Token: 0x0400042E RID: 1070
        public const uint CPU_VORTEX86_MX = 536870916u;

        // Token: 0x0400042F RID: 1071
        public const uint CPU_VORTEX86_DX3 = 536870917u;

        // Token: 0x04000430 RID: 1072
        public const int CACHE_TYPE_DATA = 1;

        // Token: 0x04000431 RID: 1073
        public const int CACHE_TYPE_INSTRUCTION = 2;

        // Token: 0x04000432 RID: 1074
        public const int CACHE_TYPE_UNIFIED = 3;

        // Token: 0x04000433 RID: 1075
        public const int CACHE_TYPE_TRACE_CACHE = 4;

        // Token: 0x04000434 RID: 1076
        public const int ISET_MMX = 1;

        // Token: 0x04000435 RID: 1077
        public const int ISET_EXTENDED_MMX = 2;

        // Token: 0x04000436 RID: 1078
        public const int ISET_3DNOW = 3;

        // Token: 0x04000437 RID: 1079
        public const int ISET_EXTENDED_3DNOW = 4;

        // Token: 0x04000438 RID: 1080
        public const int ISET_SSE = 5;

        // Token: 0x04000439 RID: 1081
        public const int ISET_SSE2 = 6;

        // Token: 0x0400043A RID: 1082
        public const int ISET_SSE3 = 7;

        // Token: 0x0400043B RID: 1083
        public const int ISET_SSSE3 = 8;

        // Token: 0x0400043C RID: 1084
        public const int ISET_SSE4_1 = 9;

        // Token: 0x0400043D RID: 1085
        public const int ISET_SSE4_2 = 12;

        // Token: 0x0400043E RID: 1086
        public const int ISET_SSE4A = 13;

        // Token: 0x0400043F RID: 1087
        public const int ISET_XOP = 14;

        // Token: 0x04000440 RID: 1088
        public const int ISET_X86_64 = 16;

        // Token: 0x04000441 RID: 1089
        public const int ISET_NX = 17;

        // Token: 0x04000442 RID: 1090
        public const int ISET_VMX = 18;

        // Token: 0x04000443 RID: 1091
        public const int ISET_AES = 19;

        // Token: 0x04000444 RID: 1092
        public const int ISET_AVX = 20;

        // Token: 0x04000445 RID: 1093
        public const int ISET_AVX2 = 21;

        // Token: 0x04000446 RID: 1094
        public const int ISET_FMA3 = 22;

        // Token: 0x04000447 RID: 1095
        public const int ISET_FMA4 = 23;

        // Token: 0x04000448 RID: 1096
        public const int ISET_RTM = 24;

        // Token: 0x04000449 RID: 1097
        public const int ISET_HLE = 25;

        // Token: 0x0400044A RID: 1098
        public const int ISET_AVX512F = 26;

        // Token: 0x0400044B RID: 1099
        public const int ISET_SHA = 27;

        // Token: 0x0400044C RID: 1100
        public const int HWM_CLASS_LPC = 1;

        // Token: 0x0400044D RID: 1101
        public const int HWM_CLASS_CPU = 2;

        // Token: 0x0400044E RID: 1102
        public const int HWM_CLASS_HDD = 4;

        // Token: 0x0400044F RID: 1103
        public const int HWM_CLASS_DISPLAYADAPTER = 8;

        // Token: 0x04000450 RID: 1104
        public const int HWM_CLASS_PSU = 16;

        // Token: 0x04000451 RID: 1105
        public const int HWM_CLASS_ACPI = 32;

        // Token: 0x04000452 RID: 1106
        public const int HWM_CLASS_RAM = 64;

        // Token: 0x04000453 RID: 1107
        public const int HWM_CLASS_CHASSIS = 128;

        // Token: 0x04000454 RID: 1108
        public const int HWM_CLASS_WATERCOOLER = 256;

        // Token: 0x04000455 RID: 1109
        public const int HWM_CLASS_BATTERY = 512;

        // Token: 0x04000456 RID: 1110
        public const int SENSOR_CLASS_VOLTAGE = 4096;

        // Token: 0x04000457 RID: 1111
        public const int SENSOR_CLASS_TEMPERATURE = 8192;

        // Token: 0x04000458 RID: 1112
        public const int SENSOR_CLASS_FAN = 12288;

        // Token: 0x04000459 RID: 1113
        public const int SENSOR_CLASS_CURRENT = 16384;

        // Token: 0x0400045A RID: 1114
        public const int SENSOR_CLASS_POWER = 20480;

        // Token: 0x0400045B RID: 1115
        public const int SENSOR_CLASS_FAN_PWM = 24576;

        // Token: 0x0400045C RID: 1116
        public const int SENSOR_CLASS_PUMP_PWM = 28672;

        // Token: 0x0400045D RID: 1117
        public const int SENSOR_CLASS_WATER_LEVEL = 32768;

        // Token: 0x0400045E RID: 1118
        public const int SENSOR_CLASS_POSITION = 36864;

        // Token: 0x0400045F RID: 1119
        public const int SENSOR_CLASS_CAPACITY = 40960;

        // Token: 0x04000460 RID: 1120
        public const int SENSOR_CLASS_CASEOPEN = 45056;

        // Token: 0x04000461 RID: 1121
        public const int SENSOR_CLASS_LEVEL = 49152;

        // Token: 0x04000462 RID: 1122
        public const int SENSOR_CLASS_COUNTER = 53248;

        // Token: 0x04000463 RID: 1123
        public const int SENSOR_CLASS_UTILIZATION = 57344;

        // Token: 0x04000464 RID: 1124
        public const int SENSOR_CLASS_CLOCK_SPEED = 61440;

        // Token: 0x04000465 RID: 1125
        public const int SENSOR_CLASS_BANDWIDTH = 65536;

        // Token: 0x04000466 RID: 1126
        public const int SENSOR_CLASS_PERF_LIMITER = 69632;

        // Token: 0x04000467 RID: 1127
        public const int SENSOR_VOLTAGE_VCORE = 4198400;

        // Token: 0x04000468 RID: 1128
        public const int SENSOR_VOLTAGE_3V3 = 8392704;

        // Token: 0x04000469 RID: 1129
        public const int SENSOR_VOLTAGE_P5V = 12587008;

        // Token: 0x0400046A RID: 1130
        public const int SENSOR_VOLTAGE_P12V = 16781312;

        // Token: 0x0400046B RID: 1131
        public const int SENSOR_VOLTAGE_M5V = 20975616;

        // Token: 0x0400046C RID: 1132
        public const int SENSOR_VOLTAGE_M12V = 25169920;

        // Token: 0x0400046D RID: 1133
        public const int SENSOR_VOLTAGE_5VSB = 29364224;

        // Token: 0x0400046E RID: 1134
        public const int SENSOR_VOLTAGE_DRAM = 33558528;

        // Token: 0x0400046F RID: 1135
        public const int SENSOR_VOLTAGE_CPU_VTT = 37752832;

        // Token: 0x04000470 RID: 1136
        public const int SENSOR_VOLTAGE_IOH_VCORE = 41947136;

        // Token: 0x04000471 RID: 1137
        public const int SENSOR_VOLTAGE_IOH_PLL = 46141440;

        // Token: 0x04000472 RID: 1138
        public const int SENSOR_VOLTAGE_CPU_PLL = 50335744;

        // Token: 0x04000473 RID: 1139
        public const int SENSOR_VOLTAGE_PCH = 54530048;

        // Token: 0x04000474 RID: 1140
        public const int SENSOR_VOLTAGE_CPU_VID = 58724352;

        // Token: 0x04000475 RID: 1141
        public const int SENSOR_VOLTAGE_GPU = 62918656;

        // Token: 0x04000476 RID: 1142
        public const int SENSOR_TEMPERATURE_CPU = 4202496;

        // Token: 0x04000477 RID: 1143
        public const int SENSOR_TEMPERATURE_VREG = 8396800;

        // Token: 0x04000478 RID: 1144
        public const int SENSOR_TEMPERATURE_DRAM = 12591104;

        // Token: 0x04000479 RID: 1145
        public const int SENSOR_TEMPERATURE_PCH = 16785408;

        // Token: 0x0400047A RID: 1146
        public const int SENSOR_TEMPERATURE_GPU = 20979712;

        // Token: 0x0400047B RID: 1147
        public const int SENSOR_TEMPERATURE_CPU_DTS = 25174016;

        // Token: 0x0400047C RID: 1148
        public const int SENSOR_FAN_CPU = 4206592;

        // Token: 0x0400047D RID: 1149
        public const int SENSOR_FAN_GPU = 25178112;

        // Token: 0x0400047E RID: 1150
        public const int SENSOR_POWER_CPU = 4214784;

        // Token: 0x0400047F RID: 1151
        public const int SENSOR_POWER_CORE = 8409088;

        // Token: 0x04000480 RID: 1152
        public const int SENSOR_POWER_CPU_GT = 12603392;

        // Token: 0x04000481 RID: 1153
        public const int SENSOR_POWER_GPU = 25186304;

        // Token: 0x04000482 RID: 1154
        public const int SENSOR_UTILIZATION_CPU = 4251648;

        // Token: 0x04000483 RID: 1155
        public const int SENSOR_UTILIZATION_GPU = 25223168;

        // Token: 0x04000484 RID: 1156
        public const int MEMORY_TYPE_SPM_RAM = 1;

        // Token: 0x04000485 RID: 1157
        public const int MEMORY_TYPE_RDRAM = 2;

        // Token: 0x04000486 RID: 1158
        public const int MEMORY_TYPE_EDO_RAM = 3;

        // Token: 0x04000487 RID: 1159
        public const int MEMORY_TYPE_FPM_RAM = 4;

        // Token: 0x04000488 RID: 1160
        public const int MEMORY_TYPE_SDRAM = 5;

        // Token: 0x04000489 RID: 1161
        public const int MEMORY_TYPE_DDR_SDRAM = 6;

        // Token: 0x0400048A RID: 1162
        public const int MEMORY_TYPE_DDR2_SDRAM = 7;

        // Token: 0x0400048B RID: 1163
        public const int MEMORY_TYPE_DDR2_SDRAM_FB = 8;

        // Token: 0x0400048C RID: 1164
        public const int MEMORY_TYPE_DDR3_SDRAM = 9;

        // Token: 0x0400048D RID: 1165
        public const int MEMORY_TYPE_DDR4_SDRAM = 10;

        // Token: 0x0400048E RID: 1166
        public const int DISPLAY_CLOCK_DOMAIN_GRAPHICS = 0;

        // Token: 0x0400048F RID: 1167
        public const int DISPLAY_CLOCK_DOMAIN_MEMORY = 4;

        // Token: 0x04000490 RID: 1168
        public const int DISPLAY_CLOCK_DOMAIN_PROCESSOR = 7;

        // Token: 0x04000491 RID: 1169
        public const int MEMORY_TYPE_SDR = 1;

        // Token: 0x04000492 RID: 1170
        public const int MEMORY_TYPE_DDR = 2;

        // Token: 0x04000493 RID: 1171
        public const int MEMORY_TYPE_LPDDR2 = 9;

        // Token: 0x04000494 RID: 1172
        public const int MEMORY_TYPE_DDR2 = 3;

        // Token: 0x04000495 RID: 1173
        public const int MEMORY_TYPE_DDR3 = 7;

        // Token: 0x04000496 RID: 1174
        public const int MEMORY_TYPE_GDDR2 = 4;

        // Token: 0x04000497 RID: 1175
        public const int MEMORY_TYPE_GDDR3 = 5;

        // Token: 0x04000498 RID: 1176
        public const int MEMORY_TYPE_GDDR4 = 6;

        // Token: 0x04000499 RID: 1177
        public const int MEMORY_TYPE_GDDR5 = 8;

        // Token: 0x0400049A RID: 1178
        public const int MEMORY_TYPE_GDDR5X = 10;

        // Token: 0x0400049B RID: 1179
        public const int MEMORY_TYPE_HBM1 = 11;

        // Token: 0x0400049C RID: 1180
        public const int MEMORY_TYPE_HBM2 = 12;

        // Token: 0x0400049D RID: 1181
        public const int MEMORY_TYPE_SDDR4 = 13;

        // Token: 0x0400049E RID: 1182
        public const int MEMORY_TYPE_GDDR6 = 14;

        // Token: 0x0400049F RID: 1183
        public const int DRIVE_FEATURE_IS_SSD = 1;

        // Token: 0x040004A0 RID: 1184
        public const int DRIVE_FEATURE_SMART = 2;

        // Token: 0x040004A1 RID: 1185
        public const int DRIVE_FEATURE_TRIM = 4;

        // Token: 0x040004A2 RID: 1186
        public const int BUS_TYPE_SCSI = 1;

        // Token: 0x040004A3 RID: 1187
        public const int BUS_TYPE_ATAPI = 2;

        // Token: 0x040004A4 RID: 1188
        public const int BUS_TYPE_ATA = 3;

        // Token: 0x040004A5 RID: 1189
        public const int BUS_TYPE_IEEE1394 = 4;

        // Token: 0x040004A6 RID: 1190
        public const int BUS_TYPE_SSA = 5;

        // Token: 0x040004A7 RID: 1191
        public const int BUS_TYPE_FIBRE = 6;

        // Token: 0x040004A8 RID: 1192
        public const int BUS_TYPE_USB = 7;

        // Token: 0x040004A9 RID: 1193
        public const int BUS_TYPE_RAID = 8;

        // Token: 0x040004AA RID: 1194
        public const int BUS_TYPE_ISCSI = 9;

        // Token: 0x040004AB RID: 1195
        public const int BUS_TYPE_SAS = 10;

        // Token: 0x040004AC RID: 1196
        public const int BUS_TYPE_SATA = 11;

        // Token: 0x040004AD RID: 1197
        public const int BUS_TYPE_SD = 12;

        // Token: 0x040004AE RID: 1198
        public const int BUS_TYPE_MMC = 13;

        // Token: 0x040004AF RID: 1199
        public const int BUS_TYPE_VIRTUAL = 14;

        // Token: 0x040004B0 RID: 1200
        public const int BUS_TYPE_FILEBACKEDVIRTUAL = 15;

        // Token: 0x040004B1 RID: 1201
        public const int BUS_TYPE_SPACES = 16;

        // Token: 0x040004B2 RID: 1202
        public const int BUS_TYPE_NVME = 17;

        // Token: 0x040004B3 RID: 1203
        public const string szDllPath = "..\\..\\..\\..\\..\\bin\\";

        // Token: 0x040004B4 RID: 1204
        public const string szDllFilename = "cpuidsdk.dll";

        // Token: 0x040004B5 RID: 1205
        protected const string szDllName = "..\\..\\..\\..\\..\\bin\\cpuidsdk.dll";

        // Token: 0x040004B6 RID: 1206
        protected IntPtr objptr = IntPtr.Zero;

        // Token: 0x040004B7 RID: 1207
        private IntPtr _dllHandle = IntPtr.Zero;

        // Token: 0x040004B8 RID: 1208
        private CPUIDSDK.CPUIDSDKfpQueryInterface CPUIDSDK_fp_QueryInterface;

        // Token: 0x02000157 RID: 343
        private enum PTR : uint
        {
            // Token: 0x0400091C RID: 2332
            PTR0 = 3114496838u,
            // Token: 0x0400091D RID: 2333
            PTR1 = 141955308u,
            // Token: 0x0400091E RID: 2334
            PTR2 = 1642906585u,
            // Token: 0x0400091F RID: 2335
            PTR3 = 2482448366u,
            // Token: 0x04000920 RID: 2336
            PTR4 = 2716681178u,
            // Token: 0x04000921 RID: 2337
            PTR5 = 1583791309u,
            // Token: 0x04000922 RID: 2338
            PTR6 = 2244545426u,
            // Token: 0x04000923 RID: 2339
            PTR7 = 4120898367u,
            // Token: 0x04000924 RID: 2340
            PTR8 = 1940580181u,
            // Token: 0x04000925 RID: 2341
            PTR9 = 1069580160u,
            // Token: 0x04000926 RID: 2342
            PTR10 = 3357642819u,
            // Token: 0x04000927 RID: 2343
            PTR11 = 259923708u,
            // Token: 0x04000928 RID: 2344
            PTR12 = 2057106745u,
            // Token: 0x04000929 RID: 2345
            PTR13 = 3856386999u,
            // Token: 0x0400092A RID: 2346
            PTR14 = 2738439794u,
            // Token: 0x0400092B RID: 2347
            PTR15 = 3458309187u,
            // Token: 0x0400092C RID: 2348
            PTR16 = 210508056u,
            // Token: 0x0400092D RID: 2349
            PTR17 = 637422588u,
            // Token: 0x0400092E RID: 2350
            PTR18 = 1068924780u,
            // Token: 0x0400092F RID: 2351
            PTR19 = 235543572u,
            // Token: 0x04000930 RID: 2352
            PTR20 = 2313360326u,
            // Token: 0x04000931 RID: 2353
            PTR21 = 851731848u,
            // Token: 0x04000932 RID: 2354
            PTR22 = 1540798381u,
            // Token: 0x04000933 RID: 2355
            PTR23 = 2054878453u,
            // Token: 0x04000934 RID: 2356
            PTR24 = 3932017851u,
            // Token: 0x04000935 RID: 2357
            PTR25 = 3552159603u,
            // Token: 0x04000936 RID: 2358
            PTR26 = 3839609271u,
            // Token: 0x04000937 RID: 2359
            PTR27 = 1909121941u,
            // Token: 0x04000938 RID: 2360
            PTR28 = 152310312u,
            // Token: 0x04000939 RID: 2361
            PTR29 = 2019750085u,
            // Token: 0x0400093A RID: 2362
            PTR30 = 3512574651u,
            // Token: 0x0400093B RID: 2363
            PTR31 = 3522929655u,
            // Token: 0x0400093C RID: 2364
            PTR32 = 3093000374u,
            // Token: 0x0400093D RID: 2365
            PTR33 = 1468968733u,
            // Token: 0x0400093E RID: 2366
            PTR34 = 3666326799u,
            // Token: 0x0400093F RID: 2367
            PTR35 = 1443015685u,
            // Token: 0x04000940 RID: 2368
            PTR36 = 3194846426u,
            // Token: 0x04000941 RID: 2369
            PTR37 = 2473273046u,
            // Token: 0x04000942 RID: 2370
            PTR38 = 1746718777u,
            // Token: 0x04000943 RID: 2371
            PTR39 = 504904752u,
            // Token: 0x04000944 RID: 2372
            PTR40 = 4267441335u,
            // Token: 0x04000945 RID: 2373
            PTR41 = 2044130221u,
            // Token: 0x04000946 RID: 2374
            PTR42 = 2796113234u,
            // Token: 0x04000947 RID: 2375
            PTR43 = 389033568u,
            // Token: 0x04000948 RID: 2376
            PTR44 = 3228270807u,
            // Token: 0x04000949 RID: 2377
            PTR45 = 29754252u,
            // Token: 0x0400094A RID: 2378
            PTR46 = 907308072u,
            // Token: 0x0400094B RID: 2379
            PTR47 = 3219226562u,
            // Token: 0x0400094C RID: 2380
            PTR48 = 1526511097u,
            // Token: 0x0400094D RID: 2381
            PTR49 = 1590214033u,
            // Token: 0x0400094E RID: 2382
            PTR50 = 2518625342u,
            // Token: 0x0400094F RID: 2383
            PTR51 = 2529111422u,
            // Token: 0x04000950 RID: 2384
            PTR52 = 3296430327u,
            // Token: 0x04000951 RID: 2385
            PTR53 = 256384656u,
            // Token: 0x04000952 RID: 2386
            PTR54 = 3075042962u,
            // Token: 0x04000953 RID: 2387
            PTR55 = 206051472u,
            // Token: 0x04000954 RID: 2388
            PTR56 = 1215598825u,
            // Token: 0x04000955 RID: 2389
            PTR57 = 1866522241u,
            // Token: 0x04000956 RID: 2390
            PTR58 = 771513336u,
            // Token: 0x04000957 RID: 2391
            PTR59 = 3899117775u,
            // Token: 0x04000958 RID: 2392
            PTR60 = 1827461593u,
            // Token: 0x04000959 RID: 2393
            PTR61 = 1952114869u,
            // Token: 0x0400095A RID: 2394
            PTR62 = 3822307239u,
            // Token: 0x0400095B RID: 2395
            PTR63 = 4054180683u,
            // Token: 0x0400095C RID: 2396
            PTR64 = 1021344192u,
            // Token: 0x0400095D RID: 2397
            PTR65 = 964588284u,
            // Token: 0x0400095E RID: 2398
            PTR66 = 3000985022u,
            // Token: 0x0400095F RID: 2399
            PTR67 = 4099270827u,
            // Token: 0x04000960 RID: 2400
            PTR68 = 2071918333u,
            // Token: 0x04000961 RID: 2401
            PTR69 = 2555457698u,
            // Token: 0x04000962 RID: 2402
            PTR70 = 2265124358u,
            // Token: 0x04000963 RID: 2403
            PTR71 = 1623769489u,
            // Token: 0x04000964 RID: 2404
            PTR72 = 1198952173u,
            // Token: 0x04000965 RID: 2405
            PTR73 = 3047648078u,
            // Token: 0x04000966 RID: 2406
            PTR74 = 2207713070u,
            // Token: 0x04000967 RID: 2407
            PTR75 = 1914364981u,
            // Token: 0x04000968 RID: 2408
            PTR76 = 1325440513u,
            // Token: 0x04000969 RID: 2409
            PTR77 = 1789318477u,
            // Token: 0x0400096A RID: 2410
            PTR78 = 3882340047u,
            // Token: 0x0400096B RID: 2411
            PTR79 = 3580734171u,
            // Token: 0x0400096C RID: 2412
            PTR80 = 2569220678u,
            // Token: 0x0400096D RID: 2413
            PTR81 = 3680876235u,
            // Token: 0x0400096E RID: 2414
            PTR82 = 2422939862u,
            // Token: 0x0400096F RID: 2415
            PTR83 = 1717488829u,
            // Token: 0x04000970 RID: 2416
            PTR84 = 1414834345u,
            // Token: 0x04000971 RID: 2417
            PTR85 = 1484828929u,
            // Token: 0x04000972 RID: 2418
            PTR86 = 1968761521u,
            // Token: 0x04000973 RID: 2419
            PTR87 = 869689260u,
            // Token: 0x04000974 RID: 2420
            PTR88 = 1041136668u,
            // Token: 0x04000975 RID: 2421
            PTR89 = 1571732317u,
            // Token: 0x04000976 RID: 2422
            PTR90 = 1916986501u,
            // Token: 0x04000977 RID: 2423
            PTR91 = 2754824294u,
            // Token: 0x04000978 RID: 2424
            PTR92 = 354691656u,
            // Token: 0x04000979 RID: 2425
            PTR93 = 685527480u,
            // Token: 0x0400097A RID: 2426
            PTR94 = 2331317738u,
            // Token: 0x0400097B RID: 2427
            PTR95 = 4223137647u,
            // Token: 0x0400097C RID: 2428
            PTR96 = 2500143626u,
            // Token: 0x0400097D RID: 2429
            PTR97 = 3610095195u,
            // Token: 0x0400097E RID: 2430
            PTR98 = 3806709195u,
            // Token: 0x0400097F RID: 2431
            PTR99 = 1792595377u,
            // Token: 0x04000980 RID: 2432
            PTR100 = 1178897545u,
            // Token: 0x04000981 RID: 2433
            PTR101 = 365702040u,
            // Token: 0x04000982 RID: 2434
            PTR102 = 1527166477u,
            // Token: 0x04000983 RID: 2435
            PTR103 = 1826544061u,
            // Token: 0x04000984 RID: 2436
            PTR104 = 2125135189u,
            // Token: 0x04000985 RID: 2437
            PTR105 = 3699882255u,
            // Token: 0x04000986 RID: 2438
            PTR106 = 1672529761u,
            // Token: 0x04000987 RID: 2439
            PTR107 = 2498570714u,
            // Token: 0x04000988 RID: 2440
            PTR108 = 3990870975u,
            // Token: 0x04000989 RID: 2441
            PTR109 = 2437751450u,
            // Token: 0x0400098A RID: 2442
            PTR110 = 278667576u,
            // Token: 0x0400098B RID: 2443
            PTR111 = 159781644u,
            // Token: 0x0400098C RID: 2444
            PTR112 = 881879328u,
            // Token: 0x0400098D RID: 2445
            PTR113 = 4202689791u,
            // Token: 0x0400098E RID: 2446
            PTR114 = 4281335391u,
            // Token: 0x0400098F RID: 2447
            PTR115 = 2990367866u,
            // Token: 0x04000990 RID: 2448
            PTR116 = 3672880599u,
            // Token: 0x04000991 RID: 2449
            PTR117 = 4034912511u,
            // Token: 0x04000992 RID: 2450
            PTR118 = 152179236u,
            // Token: 0x04000993 RID: 2451
            PTR119 = 30933936u,
            // Token: 0x04000994 RID: 2452
            PTR120 = 167384052u,
            // Token: 0x04000995 RID: 2453
            PTR121 = 3780493995u,
            // Token: 0x04000996 RID: 2454
            PTR122 = 1019115900u,
            // Token: 0x04000997 RID: 2455
            PTR123 = 1046248632u,
            // Token: 0x04000998 RID: 2456
            PTR124 = 3610095195u,
            // Token: 0x04000999 RID: 2457
            PTR125 = 1327930957u,
            // Token: 0x0400099A RID: 2458
            PTR126 = 2764261766u,
            // Token: 0x0400099B RID: 2459
            PTR127 = 2286227594u,
            // Token: 0x0400099C RID: 2460
            PTR128 = 3124196462u,
            // Token: 0x0400099D RID: 2461
            PTR129 = 2991940778u,
            // Token: 0x0400099E RID: 2462
            PTR130 = 2379422630u,
            // Token: 0x0400099F RID: 2463
            PTR131 = 4293132231u,
            // Token: 0x040009A0 RID: 2464
            PTR132 = 3539969535u,
            // Token: 0x040009A1 RID: 2465
            PTR133 = 477116640u,
            // Token: 0x040009A2 RID: 2466
            PTR134 = 1027242612u,
            // Token: 0x040009A3 RID: 2467
            PTR135 = 150082020u,
            // Token: 0x040009A4 RID: 2468
            PTR136 = 2846577494u,
            // Token: 0x040009A5 RID: 2469
            PTR137 = 4291297167u,
            // Token: 0x040009A6 RID: 2470
            PTR138 = 1110475873u,
            // Token: 0x040009A7 RID: 2471
            PTR139 = 3537348015u,
            // Token: 0x040009A8 RID: 2472
            PTR140 = 3205856810u,
            // Token: 0x040009A9 RID: 2473
            PTR141 = 3624906783u,
            // Token: 0x040009AA RID: 2474
            PTR142 = 2148073490u,
            // Token: 0x040009AB RID: 2475
            PTR143 = 337389624u,
            // Token: 0x040009AC RID: 2476
            PTR144 = 4142263755u,
            // Token: 0x040009AD RID: 2477
            PTR145 = 526401216u,
            // Token: 0x040009AE RID: 2478
            PTR146 = 700076916u,
            // Token: 0x040009AF RID: 2479
            PTR147 = 2573546186u,
            // Token: 0x040009B0 RID: 2480
            PTR148 = 1713163321u,
            // Token: 0x040009B1 RID: 2481
            PTR149 = 1170901909u,
            // Token: 0x040009B2 RID: 2482
            PTR150 = 1972300573u,
            // Token: 0x040009B3 RID: 2483
            PTR151 = 2268794486u,
            // Token: 0x040009B4 RID: 2484
            PTR152 = 1781060689u,
            // Token: 0x040009B5 RID: 2485
            PTR153 = 2999018882u,
            // Token: 0x040009B6 RID: 2486
            PTR154 = 2319520898u,
            // Token: 0x040009B7 RID: 2487
            PTR155 = 750279024u,
            // Token: 0x040009B8 RID: 2488
            PTR156 = 980841708u,
            // Token: 0x040009B9 RID: 2489
            PTR157 = 3653874579u,
            // Token: 0x040009BA RID: 2490
            PTR158 = 3579816639u,
            // Token: 0x040009BB RID: 2491
            PTR159 = 4154191671u,
            // Token: 0x040009BC RID: 2492
            PTR160 = 570835980u,
            // Token: 0x040009BD RID: 2493
            PTR161 = 1004959692u,
            // Token: 0x040009BE RID: 2494
            PTR162 = 2102327965u,
            // Token: 0x040009BF RID: 2495
            PTR163 = 1562294845u,
            // Token: 0x040009C0 RID: 2496
            PTR164 = 3314387739u,
            // Token: 0x040009C1 RID: 2497
            PTR165 = 2743682834u,
            // Token: 0x040009C2 RID: 2498
            PTR166 = 3937260891u,
            // Token: 0x040009C3 RID: 2499
            PTR167 = 2874496682u,
            // Token: 0x040009C4 RID: 2500
            PTR168 = 1222939081u,
            // Token: 0x040009C5 RID: 2501
            PTR169 = 2024862049u,
            // Token: 0x040009C6 RID: 2502
            PTR170 = 84281868u,
            // Token: 0x040009C7 RID: 2503
            PTR171 = 2972410454u,
            // Token: 0x040009C8 RID: 2504
            PTR172 = 1092780613u,
            // Token: 0x040009C9 RID: 2505
            PTR173 = 3367080291u,
            // Token: 0x040009CA RID: 2506
            PTR174 = 3392771187u,
            // Token: 0x040009CB RID: 2507
            PTR175 = 3223552071u,
            // Token: 0x040009CC RID: 2508
            PTR176 = 3315567423u,
            // Token: 0x040009CD RID: 2509
            PTR177 = 2971755074u,
            // Token: 0x040009CE RID: 2510
            PTR178 = 3013306166u,
            // Token: 0x040009CF RID: 2511
            PTR179 = 2691383510u,
            // Token: 0x040009D0 RID: 2512
            PTR180 = 350890452u,
            // Token: 0x040009D1 RID: 2513
            PTR181 = 1230672565u,
            // Token: 0x040009D2 RID: 2514
            PTR182 = 1438690177u,
            // Token: 0x040009D3 RID: 2515
            PTR183 = 2646817670u,
            // Token: 0x040009D4 RID: 2516
            PTR184 = 2741716694u,
            // Token: 0x040009D5 RID: 2517
            PTR185 = 184817160u,
            // Token: 0x040009D6 RID: 2518
            PTR186 = 2509712174u,
            // Token: 0x040009D7 RID: 2519
            PTR187 = 2696233322u,
            // Token: 0x040009D8 RID: 2520
            PTR188 = 2389646558u,
            // Token: 0x040009D9 RID: 2521
            PTR189 = 1762447897u,
            // Token: 0x040009DA RID: 2522
            PTR190 = 4238866767u,
            // Token: 0x040009DB RID: 2523
            PTR191 = 1942808473u,
            // Token: 0x040009DC RID: 2524
            PTR192 = 1980165133u,
            // Token: 0x040009DD RID: 2525
            PTR193 = 3112530698u,
            // Token: 0x040009DE RID: 2526
            PTR194 = 1661519377u,
            // Token: 0x040009DF RID: 2527
            PTR195 = 2640394946u,
            // Token: 0x040009E0 RID: 2528
            PTR196 = 2582590430u,
            // Token: 0x040009E1 RID: 2529
            PTR197 = 1950673033u,
            // Token: 0x040009E2 RID: 2530
            PTR198 = 1592835553u,
            // Token: 0x040009E3 RID: 2531
            PTR199 = 3351089019u,
            // Token: 0x040009E4 RID: 2532
            PTR200 = 856188432u,
            // Token: 0x040009E5 RID: 2533
            PTR201 = 2560831814u,
            // Token: 0x040009E6 RID: 2534
            PTR202 = 2291863862u,
            // Token: 0x040009E7 RID: 2535
            PTR203 = 2872399466u,
            // Token: 0x040009E8 RID: 2536
            PTR204 = 1204195213u,
            // Token: 0x040009E9 RID: 2537
            PTR205 = 2819837990u,
            // Token: 0x040009EA RID: 2538
            PTR206 = 1444719673u,
            // Token: 0x040009EB RID: 2539
            PTR207 = 2557948142u,
            // Token: 0x040009EC RID: 2540
            PTR208 = 1246663837u,
            // Token: 0x040009ED RID: 2541
            PTR209 = 745560288u,
            // Token: 0x040009EE RID: 2542
            PTR210 = 346171716u,
            // Token: 0x040009EF RID: 2543
            PTR211 = 1968892597u,
            // Token: 0x040009F0 RID: 2544
            PTR212 = 2344294262u,
            // Token: 0x040009F1 RID: 2545
            PTR213 = 2054747377u,
            // Token: 0x040009F2 RID: 2546
            PTR214 = 3705518523u,
            // Token: 0x040009F3 RID: 2547
            PTR215 = 239475852u,
            // Token: 0x040009F4 RID: 2548
            PTR216 = 168563736u,
            // Token: 0x040009F5 RID: 2549
            PTR217 = 3970947423u,
            // Token: 0x040009F6 RID: 2550
            PTR218 = 206706852u,
            // Token: 0x040009F7 RID: 2551
            PTR219 = 703484892u,
            // Token: 0x040009F8 RID: 2552
            PTR220 = 478689552u,
            // Token: 0x040009F9 RID: 2553
            PTR221 = 4163104839u,
            // Token: 0x040009FA RID: 2554
            PTR222 = 4243323351u,
            // Token: 0x040009FB RID: 2555
            PTR223 = 3960068115u,
            // Token: 0x040009FC RID: 2556
            PTR224 = 599934852u,
            // Token: 0x040009FD RID: 2557
            PTR225 = 3194977502u,
            // Token: 0x040009FE RID: 2558
            PTR226 = 440022132u,
            // Token: 0x040009FF RID: 2559
            PTR227 = 3447167727u,
            // Token: 0x04000A00 RID: 2560
            PTR228 = 2726511878u,
            // Token: 0x04000A01 RID: 2561
            PTR229 = 2134441585u,
            // Token: 0x04000A02 RID: 2562
            PTR230 = 2275086134u
        }

        // Token: 0x02000158 RID: 344
        // (Invoke) Token: 0x06000BB5 RID: 2997
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr CPUIDSDKfpQueryInterface(uint _code);

        // Token: 0x02000159 RID: 345
        // (Invoke) Token: 0x06000BB9 RID: 3001
        private delegate IntPtr CPUIDSDK_fp_CreateInstance();

        // Token: 0x0200015A RID: 346
        // (Invoke) Token: 0x06000BBD RID: 3005
        private delegate void CPUIDSDK_fp_DestroyInstance(IntPtr objptr);

        // Token: 0x0200015B RID: 347
        // (Invoke) Token: 0x06000BC1 RID: 3009
        private delegate int CPUIDSDK_fp_Init(IntPtr objptr, string _szDllPath, string _szDllFilename, int _config_flag, ref int _errorcode, ref int _extended_errorcode);

        // Token: 0x0200015C RID: 348
        // (Invoke) Token: 0x06000BC5 RID: 3013
        private delegate void CPUIDSDK_fp_Close(IntPtr objptr);

        // Token: 0x0200015D RID: 349
        // (Invoke) Token: 0x06000BC9 RID: 3017
        private delegate void CPUIDSDK_fp_RefreshInformation(IntPtr objptr);

        // Token: 0x0200015E RID: 350
        // (Invoke) Token: 0x06000BCD RID: 3021
        private delegate void CPUIDSDK_fp_GetDllVersion(IntPtr objptr, ref int _version);

        // Token: 0x0200015F RID: 351
        // (Invoke) Token: 0x06000BD1 RID: 3025
        private delegate int CPUIDSDK_fp_GetNbProcessors(IntPtr objptr);

        // Token: 0x02000160 RID: 352
        // (Invoke) Token: 0x06000BD5 RID: 3029
        private delegate int CPUIDSDK_fp_GetProcessorFamily(IntPtr objptr, int _proc_index);

        // Token: 0x02000161 RID: 353
        // (Invoke) Token: 0x06000BD9 RID: 3033
        private delegate int CPUIDSDK_fp_GetProcessorCoreCount(IntPtr objptr, int _proc_index);

        // Token: 0x02000162 RID: 354
        // (Invoke) Token: 0x06000BDD RID: 3037
        private delegate int CPUIDSDK_fp_GetProcessorThreadCount(IntPtr objptr, int _proc_index);

        // Token: 0x02000163 RID: 355
        // (Invoke) Token: 0x06000BE1 RID: 3041
        private delegate int CPUIDSDK_fp_GetProcessorCoreThreadCount(IntPtr objptr, int _proc_index, int _core_index);

        // Token: 0x02000164 RID: 356
        // (Invoke) Token: 0x06000BE5 RID: 3045
        private delegate int CPUIDSDK_fp_GetProcessorThreadAPICID(IntPtr objptr, int _proc_index, int _core_index, int _thread_index);

        // Token: 0x02000165 RID: 357
        // (Invoke) Token: 0x06000BE9 RID: 3049
        private delegate IntPtr CPUIDSDK_fp_GetProcessorName(IntPtr objptr, int _proc_index);

        // Token: 0x02000166 RID: 358
        // (Invoke) Token: 0x06000BED RID: 3053
        private delegate IntPtr CPUIDSDK_fp_GetProcessorCodeName(IntPtr objptr, int _proc_index);

        // Token: 0x02000167 RID: 359
        // (Invoke) Token: 0x06000BF1 RID: 3057
        private delegate IntPtr CPUIDSDK_fp_GetProcessorSpecification(IntPtr objptr, int _proc_index);

        // Token: 0x02000168 RID: 360
        // (Invoke) Token: 0x06000BF5 RID: 3061
        private delegate IntPtr CPUIDSDK_fp_GetProcessorPackage(IntPtr objptr, int _proc_index);

        // Token: 0x02000169 RID: 361
        // (Invoke) Token: 0x06000BF9 RID: 3065
        private delegate IntPtr CPUIDSDK_fp_GetProcessorStepping(IntPtr objptr, int _proc_index);

        // Token: 0x0200016A RID: 362
        // (Invoke) Token: 0x06000BFD RID: 3069
        private delegate float CPUIDSDK_fp_GetProcessorTDP(IntPtr objptr, int _proc_index);

        // Token: 0x0200016B RID: 363
        // (Invoke) Token: 0x06000C01 RID: 3073
        private delegate float CPUIDSDK_fp_GetProcessorManufacturingProcess(IntPtr objptr, int _proc_index);

        // Token: 0x0200016C RID: 364
        // (Invoke) Token: 0x06000C05 RID: 3077
        private delegate int CPUIDSDK_fp_IsProcessorInstructionSetAvailable(IntPtr objptr, int _proc_index, int _iset);

        // Token: 0x0200016D RID: 365
        // (Invoke) Token: 0x06000C09 RID: 3081
        private delegate float CPUIDSDK_fp_GetProcessorCoreClockFrequency(IntPtr objptr, int _proc_index, int _core_index);

        // Token: 0x0200016E RID: 366
        // (Invoke) Token: 0x06000C0D RID: 3085
        private delegate float CPUIDSDK_fp_GetProcessorCoreClockMultiplier(IntPtr objptr, int _proc_index, int _core_index);

        // Token: 0x0200016F RID: 367
        // (Invoke) Token: 0x06000C11 RID: 3089
        private delegate float CPUIDSDK_fp_GetProcessorCoreTemperature(IntPtr objptr, int _proc_index, int _core_index);

        // Token: 0x02000170 RID: 368
        // (Invoke) Token: 0x06000C15 RID: 3093
        private delegate float CPUIDSDK_fp_GetBusFrequency(IntPtr objptr);

        // Token: 0x02000171 RID: 369
        // (Invoke) Token: 0x06000C19 RID: 3097
        private delegate float CPUIDSDK_fp_GetProcessorRatedBusFrequency(IntPtr objptr, int _proc_index);

        // Token: 0x02000172 RID: 370
        // (Invoke) Token: 0x06000C1D RID: 3101
        private delegate int CPUIDSDK_fp_GetProcessorStockClockFrequency(IntPtr objptr, int _proc_index);

        // Token: 0x02000173 RID: 371
        // (Invoke) Token: 0x06000C21 RID: 3105
        private delegate int CPUIDSDK_fp_GetProcessorStockBusFrequency(IntPtr objptr, int _proc_index);

        // Token: 0x02000174 RID: 372
        // (Invoke) Token: 0x06000C25 RID: 3109
        private delegate int CPUIDSDK_fp_GetProcessorMaxCacheLevel(IntPtr objptr, int _proc_index);

        // Token: 0x02000175 RID: 373
        // (Invoke) Token: 0x06000C29 RID: 3113
        private delegate void CPUIDSDK_fp_GetProcessorCacheParameters(IntPtr objptr, int _proc_index, int _cache_level, int _cache_type, ref int _NbCaches, ref int _size);

        // Token: 0x02000176 RID: 374
        // (Invoke) Token: 0x06000C2D RID: 3117
        private delegate int CPUIDSDK_fp_GetProcessorID(IntPtr objptr, int _proc_index);

        // Token: 0x02000177 RID: 375
        // (Invoke) Token: 0x06000C31 RID: 3121
        private delegate float CPUIDSDK_fp_GetProcessorVoltageID(IntPtr objptr, int _proc_index);

        // Token: 0x02000178 RID: 376
        // (Invoke) Token: 0x06000C35 RID: 3125
        private delegate int CPUIDSDK_fp_GetMemoryType(IntPtr objptr);

        // Token: 0x02000179 RID: 377
        // (Invoke) Token: 0x06000C39 RID: 3129
        private delegate int CPUIDSDK_fp_GetMemorySize(IntPtr objptr);

        // Token: 0x0200017A RID: 378
        // (Invoke) Token: 0x06000C3D RID: 3133
        private delegate float CPUIDSDK_fp_GetMemoryClockFrequency(IntPtr objptr);

        // Token: 0x0200017B RID: 379
        // (Invoke) Token: 0x06000C41 RID: 3137
        private delegate int CPUIDSDK_fp_GetMemoryNumberOfChannels(IntPtr objptr);

        // Token: 0x0200017C RID: 380
        // (Invoke) Token: 0x06000C45 RID: 3141
        private delegate float CPUIDSDK_fp_GetMemoryCASLatency(IntPtr objptr);

        // Token: 0x0200017D RID: 381
        // (Invoke) Token: 0x06000C49 RID: 3145
        private delegate int CPUIDSDK_fp_GetMemoryRAStoCASDelay(IntPtr objptr);

        // Token: 0x0200017E RID: 382
        // (Invoke) Token: 0x06000C4D RID: 3149
        private delegate int CPUIDSDK_fp_GetMemoryRASPrecharge(IntPtr objptr);

        // Token: 0x0200017F RID: 383
        // (Invoke) Token: 0x06000C51 RID: 3153
        private delegate int CPUIDSDK_fp_GetMemoryTRAS(IntPtr objptr);

        // Token: 0x02000180 RID: 384
        // (Invoke) Token: 0x06000C55 RID: 3157
        private delegate int CPUIDSDK_fp_GetMemoryTRC(IntPtr objptr);

        // Token: 0x02000181 RID: 385
        // (Invoke) Token: 0x06000C59 RID: 3161
        private delegate int CPUIDSDK_fp_GetMemoryCommandRate(IntPtr objptr);

        // Token: 0x02000182 RID: 386
        // (Invoke) Token: 0x06000C5D RID: 3165
        private delegate IntPtr CPUIDSDK_fp_GetNorthBridgeVendor(IntPtr objptr);

        // Token: 0x02000183 RID: 387
        // (Invoke) Token: 0x06000C61 RID: 3169
        private delegate IntPtr CPUIDSDK_fp_GetNorthBridgeModel(IntPtr objptr);

        // Token: 0x02000184 RID: 388
        // (Invoke) Token: 0x06000C65 RID: 3173
        private delegate IntPtr CPUIDSDK_fp_GetNorthBridgeRevision(IntPtr objptr);

        // Token: 0x02000185 RID: 389
        // (Invoke) Token: 0x06000C69 RID: 3177
        private delegate IntPtr CPUIDSDK_fp_GetSouthBridgeVendor(IntPtr objptr);

        // Token: 0x02000186 RID: 390
        // (Invoke) Token: 0x06000C6D RID: 3181
        private delegate IntPtr CPUIDSDK_fp_GetSouthBridgeModel(IntPtr objptr);

        // Token: 0x02000187 RID: 391
        // (Invoke) Token: 0x06000C71 RID: 3185
        private delegate IntPtr CPUIDSDK_fp_GetSouthBridgeRevision(IntPtr objptr);

        // Token: 0x02000188 RID: 392
        // (Invoke) Token: 0x06000C75 RID: 3189
        private delegate void CPUIDSDK_fp_GetGraphicBusLinkParameters(IntPtr objptr, ref int _bus_type, ref int _link_width);

        // Token: 0x02000189 RID: 393
        // (Invoke) Token: 0x06000C79 RID: 3193
        private delegate void CPUIDSDK_fp_GetMemorySlotsConfig(IntPtr objptr, ref int _nbslots, ref int _nbusedslots, ref int _slotmap_h, ref int _slotmap_l, ref int _maxmodulesize);

        // Token: 0x0200018A RID: 394
        // (Invoke) Token: 0x06000C7D RID: 3197
        private delegate IntPtr CPUIDSDK_fp_GetBIOSVendor(IntPtr objptr);

        // Token: 0x0200018B RID: 395
        // (Invoke) Token: 0x06000C81 RID: 3201
        private delegate IntPtr CPUIDSDK_fp_GetBIOSVersion(IntPtr objptr);

        // Token: 0x0200018C RID: 396
        // (Invoke) Token: 0x06000C85 RID: 3205
        private delegate IntPtr CPUIDSDK_fp_GetBIOSDate(IntPtr objptr);

        // Token: 0x0200018D RID: 397
        // (Invoke) Token: 0x06000C89 RID: 3209
        private delegate IntPtr CPUIDSDK_fp_GetMainboardVendor(IntPtr objptr);

        // Token: 0x0200018E RID: 398
        // (Invoke) Token: 0x06000C8D RID: 3213
        private delegate IntPtr CPUIDSDK_fp_GetMainboardModel(IntPtr objptr);

        // Token: 0x0200018F RID: 399
        // (Invoke) Token: 0x06000C91 RID: 3217
        private delegate IntPtr CPUIDSDK_fp_GetMainboardRevision(IntPtr objptr);

        // Token: 0x02000190 RID: 400
        // (Invoke) Token: 0x06000C95 RID: 3221
        private delegate IntPtr CPUIDSDK_fp_GetMainboardSerialNumber(IntPtr objptr);

        // Token: 0x02000191 RID: 401
        // (Invoke) Token: 0x06000C99 RID: 3225
        private delegate IntPtr CPUIDSDK_fp_GetSystemManufacturer(IntPtr objptr);

        // Token: 0x02000192 RID: 402
        // (Invoke) Token: 0x06000C9D RID: 3229
        private delegate IntPtr CPUIDSDK_fp_GetSystemProductName(IntPtr objptr);

        // Token: 0x02000193 RID: 403
        // (Invoke) Token: 0x06000CA1 RID: 3233
        private delegate IntPtr CPUIDSDK_fp_GetSystemVersion(IntPtr objptr);

        // Token: 0x02000194 RID: 404
        // (Invoke) Token: 0x06000CA5 RID: 3237
        private delegate IntPtr CPUIDSDK_fp_GetSystemSerialNumber(IntPtr objptr);

        // Token: 0x02000195 RID: 405
        // (Invoke) Token: 0x06000CA9 RID: 3241
        private delegate IntPtr CPUIDSDK_fp_GetSystemUUID(IntPtr objptr);

        // Token: 0x02000196 RID: 406
        // (Invoke) Token: 0x06000CAD RID: 3245
        private delegate IntPtr CPUIDSDK_fp_GetChassisManufacturer(IntPtr objptr);

        // Token: 0x02000197 RID: 407
        // (Invoke) Token: 0x06000CB1 RID: 3249
        private delegate IntPtr CPUIDSDK_fp_GetChassisType(IntPtr objptr);

        // Token: 0x02000198 RID: 408
        // (Invoke) Token: 0x06000CB5 RID: 3253
        private delegate IntPtr CPUIDSDK_fp_GetChassisSerialNumber(IntPtr objptr);

        // Token: 0x02000199 RID: 409
        // (Invoke) Token: 0x06000CB9 RID: 3257
        private delegate int CPUIDSDK_fp_GetMemoryInfosExt(IntPtr objptr, ref IntPtr _szLocation, ref IntPtr _szUsage, ref IntPtr _szCorrection);

        // Token: 0x0200019A RID: 410
        // (Invoke) Token: 0x06000CBD RID: 3261
        private delegate int CPUIDSDK_fp_GetNumberOfMemoryDevices(IntPtr objptr);

        // Token: 0x0200019B RID: 411
        // (Invoke) Token: 0x06000CC1 RID: 3265
        private delegate int CPUIDSDK_fp_GetMemoryDeviceInfos(IntPtr objptr, int _device_index, ref int _size, ref IntPtr _szFormat);

        // Token: 0x0200019C RID: 412
        // (Invoke) Token: 0x06000CC5 RID: 3269
        private delegate int CPUIDSDK_fp_GetMemoryDeviceInfosExt(IntPtr objptr, int _device_index, ref IntPtr _szDesignation, ref IntPtr _szType, ref int _total_width, ref int _data_width, ref int _speed);

        // Token: 0x0200019D RID: 413
        // (Invoke) Token: 0x06000CC9 RID: 3273
        private delegate int CPUIDSDK_fp_GetProcessorSockets(IntPtr objptr);

        // Token: 0x0200019E RID: 414
        // (Invoke) Token: 0x06000CCD RID: 3277
        private delegate int CPUIDSDK_fp_GetNumberOfSPDModules(IntPtr objptr);

        // Token: 0x0200019F RID: 415
        // (Invoke) Token: 0x06000CD1 RID: 3281
        private delegate int CPUIDSDK_fp_GetSPDModuleType(IntPtr objptr, int _spd_index);

        // Token: 0x020001A0 RID: 416
        // (Invoke) Token: 0x06000CD5 RID: 3285
        private delegate int CPUIDSDK_fp_GetSPDModuleSize(IntPtr objptr, int _spd_index);

        // Token: 0x020001A1 RID: 417
        // (Invoke) Token: 0x06000CD9 RID: 3289
        private delegate IntPtr CPUIDSDK_fp_GetSPDModuleFormat(IntPtr objptr, int _spd_index);

        // Token: 0x020001A2 RID: 418
        // (Invoke) Token: 0x06000CDD RID: 3293
        private delegate IntPtr CPUIDSDK_fp_GetSPDModuleManufacturer(IntPtr objptr, int _spd_index);

        // Token: 0x020001A3 RID: 419
        // (Invoke) Token: 0x06000CE1 RID: 3297
        private delegate int CPUIDSDK_fp_GetSPDModuleManufacturerID(IntPtr objptr, int _spd_index, byte[] _id);

        // Token: 0x020001A4 RID: 420
        // (Invoke) Token: 0x06000CE5 RID: 3301
        private delegate int CPUIDSDK_fp_GetSPDModuleMaxFrequency(IntPtr objptr, int _spd_index);

        // Token: 0x020001A5 RID: 421
        // (Invoke) Token: 0x06000CE9 RID: 3305
        private delegate IntPtr CPUIDSDK_fp_GetSPDModuleSpecification(IntPtr objptr, int _spd_index);

        // Token: 0x020001A6 RID: 422
        // (Invoke) Token: 0x06000CED RID: 3309
        private delegate IntPtr CPUIDSDK_fp_GetSPDModulePartNumber(IntPtr objptr, int _spd_index);

        // Token: 0x020001A7 RID: 423
        // (Invoke) Token: 0x06000CF1 RID: 3313
        private delegate IntPtr CPUIDSDK_fp_GetSPDModuleSerialNumber(IntPtr objptr, int _spd_index);

        // Token: 0x020001A8 RID: 424
        // (Invoke) Token: 0x06000CF5 RID: 3317
        private delegate float CPUIDSDK_fp_GetSPDModuleMinTRCD(IntPtr objptr, int _spd_index);

        // Token: 0x020001A9 RID: 425
        // (Invoke) Token: 0x06000CF9 RID: 3321
        private delegate float CPUIDSDK_fp_GetSPDModuleMinTRP(IntPtr objptr, int _spd_index);

        // Token: 0x020001AA RID: 426
        // (Invoke) Token: 0x06000CFD RID: 3325
        private delegate float CPUIDSDK_fp_GetSPDModuleMinTRAS(IntPtr objptr, int _spd_index);

        // Token: 0x020001AB RID: 427
        // (Invoke) Token: 0x06000D01 RID: 3329
        private delegate float CPUIDSDK_fp_GetSPDModuleMinTRC(IntPtr objptr, int _spd_index);

        // Token: 0x020001AC RID: 428
        // (Invoke) Token: 0x06000D05 RID: 3333
        private delegate int CPUIDSDK_fp_GetSPDModuleManufacturingDate(IntPtr objptr, int _spd_index, ref int _year, ref int _week);

        // Token: 0x020001AD RID: 429
        // (Invoke) Token: 0x06000D09 RID: 3337
        private delegate int CPUIDSDK_fp_GetSPDModuleNumberOfBanks(IntPtr objptr, int _spd_index);

        // Token: 0x020001AE RID: 430
        // (Invoke) Token: 0x06000D0D RID: 3341
        private delegate int CPUIDSDK_fp_GetSPDModuleDataWidth(IntPtr objptr, int _spd_index);

        // Token: 0x020001AF RID: 431
        // (Invoke) Token: 0x06000D11 RID: 3345
        private delegate int CPUIDSDK_fp_GetSPDModuleNumberOfProfiles(IntPtr objptr, int _spd_index);

        // Token: 0x020001B0 RID: 432
        // (Invoke) Token: 0x06000D15 RID: 3349
        private delegate void CPUIDSDK_fp_GetSPDModuleProfileInfos(IntPtr objptr, int _spd_index, int _profile_index, ref float _frequency, ref float _tCL, ref float _nominal_vdd);

        // Token: 0x020001B1 RID: 433
        // (Invoke) Token: 0x06000D19 RID: 3353
        private delegate int CPUIDSDK_fp_GetSPDModuleNumberOfEPPProfiles(IntPtr objptr, int _spd_index, ref int _epp_revision);

        // Token: 0x020001B2 RID: 434
        // (Invoke) Token: 0x06000D1D RID: 3357
        private delegate void CPUIDSDK_fp_GetSPDModuleEPPProfileInfos(IntPtr objptr, int _spd_index, int _profile_index, ref float _frequency, ref float _tCL, ref float _tRCD, ref float _tRAS, ref float _tRP, ref float _tRC, ref float _nominal_vdd);

        // Token: 0x020001B3 RID: 435
        // (Invoke) Token: 0x06000D21 RID: 3361
        private delegate int CPUIDSDK_fp_GetSPDModuleNumberOfXMPProfiles(IntPtr objptr, int _spd_index, ref int _xmp_revision);

        // Token: 0x020001B4 RID: 436
        // (Invoke) Token: 0x06000D25 RID: 3365
        private delegate int CPUIDSDK_fp_GetSPDModuleXMPProfileNumberOfCL(IntPtr objptr, int _spd_index, int _profile_index);

        // Token: 0x020001B5 RID: 437
        // (Invoke) Token: 0x06000D29 RID: 3369
        private delegate void CPUIDSDK_fp_GetSPDModuleXMPProfileCLInfos(IntPtr objptr, int _spd_index, int _profile_index, int _cl_index, ref float _frequency, ref float _CL);

        // Token: 0x020001B6 RID: 438
        // (Invoke) Token: 0x06000D2D RID: 3373
        private delegate void CPUIDSDK_fp_GetSPDModuleXMPProfileInfos(IntPtr objptr, int _spd_index, int _profile_index, ref float _tRCD, ref float _tRAS, ref float _tRP, ref float _nominal_vdd, ref int _max_freq, ref float _max_CL);

        // Token: 0x020001B7 RID: 439
        // (Invoke) Token: 0x06000D31 RID: 3377
        private delegate int CPUIDSDK_fp_GetSPDModuleNumberOfAMPProfiles(IntPtr objptr, int _spd_index, ref int _amp_revision);

        // Token: 0x020001B8 RID: 440
        // (Invoke) Token: 0x06000D35 RID: 3381
        private delegate void CPUIDSDK_fp_GetSPDModuleAMPProfileInfos(IntPtr objptr, int _spd_index, int _profile_index, ref int _frequency, ref float _min_cycle_time, ref float _tCL, ref float _tRCD, ref float _tRAS, ref float _tRP, ref float _tRC);

        // Token: 0x020001B9 RID: 441
        // (Invoke) Token: 0x06000D39 RID: 3385
        private delegate int CPUIDSDK_fp_GetSPDModuleRawData(IntPtr objptr, int _spd_index, int _offset);

        // Token: 0x020001BA RID: 442
        // (Invoke) Token: 0x06000D3D RID: 3389
        private delegate int CPUIDSDK_fp_GetNumberOfDevices(IntPtr objptr);

        // Token: 0x020001BB RID: 443
        // (Invoke) Token: 0x06000D41 RID: 3393
        private delegate int CPUIDSDK_fp_GetDeviceClass(IntPtr objptr, int _device_index);

        // Token: 0x020001BC RID: 444
        // (Invoke) Token: 0x06000D45 RID: 3397
        private delegate IntPtr CPUIDSDK_fp_GetDeviceName(IntPtr objptr, int _device_index);

        // Token: 0x020001BD RID: 445
        // (Invoke) Token: 0x06000D49 RID: 3401
        private delegate int CPUIDSDK_fp_GetNumberOfSensors(IntPtr objptr, int _device_index, int _sensor_class);

        // Token: 0x020001BE RID: 446
        // (Invoke) Token: 0x06000D4D RID: 3405
        private delegate int CPUIDSDK_fp_GetSensorInfos(IntPtr objptr, int _device_index, int _sensor_index, int _sensor_class, ref int _sensor_id, ref IntPtr _szNamePtr, ref int _raw_value, ref float _value, ref float _min_value, ref float _max_value);

        // Token: 0x020001BF RID: 447
        // (Invoke) Token: 0x06000D51 RID: 3409
        private delegate void CPUIDSDK_fp_SensorClearMinMax(IntPtr objptr, int _device_index, int _sensor_index, int _sensor_class);

        // Token: 0x020001C0 RID: 448
        // (Invoke) Token: 0x06000D55 RID: 3413
        private delegate float CPUIDSDK_fp_GetSensorTypeValue(IntPtr objptr, int _sensor_type, ref int _device_index, ref int _sensor_index);

        // Token: 0x020001C1 RID: 449
        // (Invoke) Token: 0x06000D59 RID: 3417
        private delegate int CPUIDSDK_fp_GetNumberOfDisplayAdapter(IntPtr objptr);

        // Token: 0x020001C2 RID: 450
        // (Invoke) Token: 0x06000D5D RID: 3421
        private delegate int CPUIDSDK_fp_GetDisplayAdapterID(IntPtr objptr, int _adapter_index);

        // Token: 0x020001C3 RID: 451
        // (Invoke) Token: 0x06000D61 RID: 3425
        private delegate IntPtr CPUIDSDK_fp_GetDisplayAdapterName(IntPtr objptr, int _adapter_index);

        // Token: 0x020001C4 RID: 452
        // (Invoke) Token: 0x06000D65 RID: 3429
        private delegate IntPtr CPUIDSDK_fp_GetDisplayAdapterCodeName(IntPtr objptr, int _adapter_index);

        // Token: 0x020001C5 RID: 453
        // (Invoke) Token: 0x06000D69 RID: 3433
        private delegate int CPUIDSDK_fp_GetDisplayAdapterNumberOfPerformanceLevels(IntPtr objptr, int _adapter_index);

        // Token: 0x020001C6 RID: 454
        // (Invoke) Token: 0x06000D6D RID: 3437
        private delegate int CPUIDSDK_fp_GetDisplayAdapterCurrentPerformanceLevel(IntPtr objptr, int _adapter_index);

        // Token: 0x020001C7 RID: 455
        // (Invoke) Token: 0x06000D71 RID: 3441
        private delegate IntPtr CPUIDSDK_fp_GetDisplayAdapterPerformanceLevelName(IntPtr objptr, int _adapter_index, int _perf_level);

        // Token: 0x020001C8 RID: 456
        // (Invoke) Token: 0x06000D75 RID: 3445
        private delegate float CPUIDSDK_fp_GetDisplayAdapterClock(IntPtr objptr, int _perf_level, int _adapter_index, int _domain);

        // Token: 0x020001C9 RID: 457
        // (Invoke) Token: 0x06000D79 RID: 3449
        private delegate float CPUIDSDK_fp_GetDisplayAdapterStockClock(IntPtr objptr, int _perf_level, int _adapter_index, int _domain);

        // Token: 0x020001CA RID: 458
        // (Invoke) Token: 0x06000D7D RID: 3453
        private delegate float CPUIDSDK_fp_GetDisplayAdapterTemperature(IntPtr objptr, int _adapter_index, int _domain);

        // Token: 0x020001CB RID: 459
        // (Invoke) Token: 0x06000D81 RID: 3457
        private delegate int CPUIDSDK_fp_GetDisplayAdapterFanSpeed(IntPtr objptr, int _adapter_index);

        // Token: 0x020001CC RID: 460
        // (Invoke) Token: 0x06000D85 RID: 3461
        private delegate int CPUIDSDK_fp_GetDisplayAdapterFanPWM(IntPtr objptr, int _adapter_index);

        // Token: 0x020001CD RID: 461
        // (Invoke) Token: 0x06000D89 RID: 3465
        private delegate int CPUIDSDK_fp_GetDisplayAdapterMemoryType(IntPtr objptr, int _adapter_index, ref int _type);

        // Token: 0x020001CE RID: 462
        // (Invoke) Token: 0x06000D8D RID: 3469
        private delegate int CPUIDSDK_fp_GetDisplayAdapterMemorySize(IntPtr objptr, int _adapter_index, ref int _size);

        // Token: 0x020001CF RID: 463
        // (Invoke) Token: 0x06000D91 RID: 3473
        private delegate int CPUIDSDK_fp_GetDisplayAdapterMemoryBusWidth(IntPtr objptr, int _adapter_index, ref int _bus_width);

        // Token: 0x020001D0 RID: 464
        // (Invoke) Token: 0x06000D95 RID: 3477
        private delegate IntPtr CPUIDSDK_fp_GetDisplayDriverVersion(IntPtr objptr);

        // Token: 0x020001D1 RID: 465
        // (Invoke) Token: 0x06000D99 RID: 3481
        private delegate IntPtr CPUIDSDK_fp_GetDirectXVersion(IntPtr objptr);

        // Token: 0x020001D2 RID: 466
        // (Invoke) Token: 0x06000D9D RID: 3485
        private delegate int CPUIDSDK_fp_GetDisplayAdapterBusInfos(IntPtr objptr, int _adapter_index, ref int _bus_type, ref int _multi_vpu);

        // Token: 0x020001D3 RID: 467
        // (Invoke) Token: 0x06000DA1 RID: 3489
        private delegate float CPUIDSDK_fp_GetDisplayAdapterManufacturingProcess(IntPtr objptr, int _adapter_index);

        // Token: 0x020001D4 RID: 468
        // (Invoke) Token: 0x06000DA5 RID: 3493
        private delegate int CPUIDSDK_fp_GetNumberOfMonitors(IntPtr objptr);

        // Token: 0x020001D5 RID: 469
        // (Invoke) Token: 0x06000DA9 RID: 3497
        private delegate IntPtr CPUIDSDK_fp_GetMonitorName(IntPtr objptr, int _monitor_index);

        // Token: 0x020001D6 RID: 470
        // (Invoke) Token: 0x06000DAD RID: 3501
        private delegate IntPtr CPUIDSDK_fp_GetMonitorVendor(IntPtr objptr, int _monitor_index);

        // Token: 0x020001D7 RID: 471
        // (Invoke) Token: 0x06000DB1 RID: 3505
        private delegate IntPtr CPUIDSDK_fp_GetMonitorID(IntPtr objptr, int _monitor_index);

        // Token: 0x020001D8 RID: 472
        // (Invoke) Token: 0x06000DB5 RID: 3509
        private delegate IntPtr CPUIDSDK_fp_GetMonitorSerial(IntPtr objptr, int _monitor_index);

        // Token: 0x020001D9 RID: 473
        // (Invoke) Token: 0x06000DB9 RID: 3513
        private delegate int CPUIDSDK_fp_GetMonitorManufacturingDate(IntPtr objptr, int _monitor_index, ref int _week, ref int _year);

        // Token: 0x020001DA RID: 474
        // (Invoke) Token: 0x06000DBD RID: 3517
        private delegate float CPUIDSDK_fp_GetMonitorSize(IntPtr objptr, int _monitor_index);

        // Token: 0x020001DB RID: 475
        // (Invoke) Token: 0x06000DC1 RID: 3521
        private delegate int CPUIDSDK_fp_GetMonitorResolution(IntPtr objptr, int _monitor_index, ref int _width, ref int _height, ref int _frequency);

        // Token: 0x020001DC RID: 476
        // (Invoke) Token: 0x06000DC5 RID: 3525
        private delegate int CPUIDSDK_fp_GetMonitorMaxPixelClock(IntPtr objptr, int _monitor_index);

        // Token: 0x020001DD RID: 477
        // (Invoke) Token: 0x06000DC9 RID: 3529
        private delegate float CPUIDSDK_fp_GetMonitorGamma(IntPtr objptr, int _monitor_index);

        // Token: 0x020001DE RID: 478
        // (Invoke) Token: 0x06000DCD RID: 3533
        private delegate int CPUIDSDK_fp_GetNumberOfStorageDevice(IntPtr objptr);

        // Token: 0x020001DF RID: 479
        // (Invoke) Token: 0x06000DD1 RID: 3537
        private delegate int CPUIDSDK_fp_GetStorageDriveNumber(IntPtr objptr, int _index);

        // Token: 0x020001E0 RID: 480
        // (Invoke) Token: 0x06000DD5 RID: 3541
        private delegate IntPtr CPUIDSDK_fp_GetStorageDeviceName(IntPtr objptr, int _index);

        // Token: 0x020001E1 RID: 481
        // (Invoke) Token: 0x06000DD9 RID: 3545
        private delegate IntPtr CPUIDSDK_fp_GetStorageDeviceRevision(IntPtr objptr, int _index);

        // Token: 0x020001E2 RID: 482
        // (Invoke) Token: 0x06000DDD RID: 3549
        private delegate IntPtr CPUIDSDK_fp_GetStorageDeviceSerialNumber(IntPtr objptr, int _index);

        // Token: 0x020001E3 RID: 483
        // (Invoke) Token: 0x06000DE1 RID: 3553
        private delegate int CPUIDSDK_fp_GetStorageDeviceBusType(IntPtr objptr, int _index);

        // Token: 0x020001E4 RID: 484
        // (Invoke) Token: 0x06000DE5 RID: 3557
        private delegate int CPUIDSDK_fp_GetStorageDeviceRotationSpeed(IntPtr objptr, int _index);

        // Token: 0x020001E5 RID: 485
        // (Invoke) Token: 0x06000DE9 RID: 3561
        private delegate int CPUIDSDK_fp_GetStorageDeviceFeatureFlag(IntPtr objptr, int _index);

        // Token: 0x020001E6 RID: 486
        // (Invoke) Token: 0x06000DED RID: 3565
        private delegate int CPUIDSDK_fp_GetStorageDeviceNumberOfVolumes(IntPtr objptr, int _index);

        // Token: 0x020001E7 RID: 487
        // (Invoke) Token: 0x06000DF1 RID: 3569
        private delegate IntPtr CPUIDSDK_fp_GetStorageDeviceVolumeLetter(IntPtr objptr, int _index, int _volume_index);

        // Token: 0x020001E8 RID: 488
        // (Invoke) Token: 0x06000DF5 RID: 3573
        private delegate float CPUIDSDK_fp_GetStorageDeviceVolumeTotalCapacity(IntPtr objptr, int _index, int _volume_index);

        // Token: 0x020001E9 RID: 489
        // (Invoke) Token: 0x06000DF9 RID: 3577
        private delegate float CPUIDSDK_fp_GetStorageDeviceVolumeAvailableCapacity(IntPtr objptr, int _index, int _volume_index);

        // Token: 0x020001EA RID: 490
        // (Invoke) Token: 0x06000DFD RID: 3581
        private delegate int CPUIDSDK_fp_GetStorageDeviceSmartAttribute(IntPtr objptr, int _index, int _attrib_index, ref int _id, ref int _flags, ref int _value, ref int _worst, byte[] _data);

        // Token: 0x020001EB RID: 491
        // (Invoke) Token: 0x06000E01 RID: 3585
        private delegate int CPUIDSDK_fp_GetStorageDevicePowerOnHours(IntPtr objptr, int _index);

        // Token: 0x020001EC RID: 492
        // (Invoke) Token: 0x06000E05 RID: 3589
        private delegate int CPUIDSDK_fp_GetStorageDevicePowerCycleCount(IntPtr objptr, int _index);

        // Token: 0x020001ED RID: 493
        // (Invoke) Token: 0x06000E09 RID: 3593
        private delegate int CPUIDSDK_fp_GenerateDump(IntPtr objptr, string _szFilePath);
    }
}
