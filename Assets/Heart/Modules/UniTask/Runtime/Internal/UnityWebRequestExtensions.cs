﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Pancake.Threading.Tasks.Internal
{
#if ENABLE_UNITYWEBREQUEST && (!UNITY_2019_1_OR_NEWER || PANCAKE_WEBREQUEST)
    
    internal static class UnityWebRequestResultExtensions
    {
        public static bool IsError(this UnityWebRequest unityWebRequest)
        {
#if UNITY_2020_2_OR_NEWER
            var result = unityWebRequest.result;
            return (result == UnityWebRequest.Result.ConnectionError)
                || (result == UnityWebRequest.Result.DataProcessingError)
                || (result == UnityWebRequest.Result.ProtocolError);
#else
            return unityWebRequest.isHttpError || unityWebRequest.isNetworkError;
#endif
        }
    }

#endif
}