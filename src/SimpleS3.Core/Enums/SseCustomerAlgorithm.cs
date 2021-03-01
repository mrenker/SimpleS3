﻿using Genbox.SimpleS3.Core.Abstracts;

namespace Genbox.SimpleS3.Core.Enums
{
    public enum SseCustomerAlgorithm
    {
        Unknown = 0,

        [EnumValue("AES256")]
        Aes256
    }
}