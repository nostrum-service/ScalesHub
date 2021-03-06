﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace ScalesHubPlugin
{
    /*
    Параметры порта: 8-бит данных, n-проверка на четность, 1-стоповый бит Данные о весе передаются постоянно ASCII кодом в обратном порядке. 
    Всего передают 7 знаков, включая запятую. 
    В начале посылки передается знак "="(8 знак) 
    (формат данных) <stx> , W7, W6 , W5 , W4, W3, W2 , W1 , SA , k , g ,<cr> ST –строб. W7,W6,W5,W4,W3,W2,W1, 
    -данные веса SA –состояние .0100ABCD CR-завершение. A=1 когда вес выходит за пределы . В=1 когда тара. С=1 когда вес устойчив. D=1 нулевое значение.  
    * */
    [Export(typeof(IDataFrame))]
    [ExportMetadata("Name", "НВТ-1")]
    [ExportMetadata("Description", "Декодер для весов НВТ-1")]
    public class DataFrameNVT1 : NxDataFrame
    {
        public DataFrameNVT1()
        {
            buffer = new byte[128];
            offset = 0;
            waiting_for = 0x3d;
        }
        private Regex mask = new Regex(@"(?<value>\d+(\.|,)*\d*)", RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        [Export(typeof(Func<byte[], bool>))]
        public override bool Decode(byte[] data)
        {
            bool result = false;
            if (offset == buffer.Length)
            {
                offset = 0;
                waiting_for = 0x3d;//ascii-код символа = (061 0x3D)  
            }
            //waiting_for = 0x20 
            //спец символ SP(пробел) 032 0x20
            //(sp).Непечатаемый символ для разделения слов или перемещения механизма печати или курсора дисплея вперед на одну позицию.

            //поиск в массиве знака =            
            for (int idx = 0; idx < data.Length;)
            {                
                buffer[offset] = data[idx];                
                
                if (data[idx] == waiting_for)
                {                    
                    String strBuffer = Encoding.ASCII.GetString(buffer, 0, offset + 1);
                    Match res = mask.Match(strBuffer);
                    String strState = String.Empty;
                    if (strBuffer.Length - 1 >= 0 && strBuffer.Length >= 1)
                        strState = strBuffer.Substring(strBuffer.Length - 2, 1);
                    else
                        strState = "@";
                    hStringState = strState;                 
                   
                    System.Diagnostics.Trace.WriteLine("StringState = " + hStringState);
                    if (strState.Contains("A"))
                        hState |= NxDeviceState.DS_ZERO;
                    else
                        hState &= ~NxDeviceState.DS_ZERO;
                    if (strState.Contains("B"))
                        hState |= NxDeviceState.DS_STEADY;
                    else
                        hState &= ~NxDeviceState.DS_STEADY;
                    if (strState.Contains("D"))
                        hState |= NxDeviceState.DS_CALIBRATION;
                    else
                        hState &= ~NxDeviceState.DS_CALIBRATION;
                    if (strState.Contains("H"))
                        hState |= NxDeviceState.DS_MAX;
                    else
                        hState &= ~NxDeviceState.DS_MAX;
                    if (strState.Contains("C"))
                        hState = NxDeviceState.DS_STEADY | NxDeviceState.DS_ZERO;
                    if (strState.Contains("E"))
                        hState = NxDeviceState.DS_CALIBRATION | NxDeviceState.DS_ZERO;
                    if (strState.Contains("F"))
                        hState = NxDeviceState.DS_STEADY | NxDeviceState.DS_ZERO;
                    if (strState.Contains("G"))
                        hState = NxDeviceState.DS_STEADY | NxDeviceState.DS_ZERO | NxDeviceState.DS_CALIBRATION;
                    if (strState.Contains("I"))
                        hState = NxDeviceState.DS_MAX | NxDeviceState.DS_ZERO;
                    if (strState.Contains("J"))
                        hState = NxDeviceState.DS_MAX | NxDeviceState.DS_STEADY;
                    if (strState.Contains("K"))
                        hState = NxDeviceState.DS_MAX | NxDeviceState.DS_STEADY | NxDeviceState.DS_ZERO;
                    if (strState.Contains("L"))
                        hState = NxDeviceState.DS_MAX | NxDeviceState.DS_CALIBRATION;
                    if (strState.Contains("M"))
                        hState = NxDeviceState.DS_MAX | NxDeviceState.DS_CALIBRATION | NxDeviceState.DS_ZERO;
                    if (strState.Contains("N"))
                        hState = NxDeviceState.DS_MAX | NxDeviceState.DS_CALIBRATION | NxDeviceState.DS_STEADY;
                    if (strState.Contains("N"))
                        hState = NxDeviceState.DS_FULL;
                    if (strState.Contains("@") || String.IsNullOrWhiteSpace(strState))
                        hState = NxDeviceState.DS_NONE;
                    ////проверить и переделать
                    if (strBuffer.Length - 7 >= 0 && strBuffer.Length >= 2)
                        hMeasure = strBuffer.Substring(strBuffer.Length - 7, 2);
                    else
                        hMeasure = String.Empty;
                    offset = 0;
                    try
                    {
                        if (res.Success)
                        {
                            String src = res.Groups["value"].Value.Replace(',', '.');
                            //развернуть строку, символы в обратном порядке
                            var reverseSrc = new String(src.Reverse().ToArray());
                            hWeight = Double.Parse(reverseSrc, CultureInfo.InvariantCulture);
                            if (strState.Contains("-"))
                                hWeight = -hWeight;
                            if (hWeight != 0) hState = NxDeviceState.DS_STEADY;
                            else hState = NxDeviceState.DS_NONE;
                            result = true;
                        }
                        else
                        {
                            hWeight = 0.0;
                            hState = NxDeviceState.DS_NONE;
                            result = false;
                        }
                    }
                    catch
                    {
                        offset = 0;
                        hWeight = 0.0;
                        result = false;
                    }
                }
                else
                {
                    offset++;
                }
                idx++;
            }
            return result;
        }
    }
}
