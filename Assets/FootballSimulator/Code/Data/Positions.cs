using System;

namespace FStudio.Data {
    [Flags]
    public enum Positions {
        GK = 1 << 1,
        RB = 1 << 2,
        LB = 1 << 3,
        CB = 1 << 4,
        CB_R = 1 << 5,
        CB_L = 1 << 6,
        DMF = 1 << 7,
        DMF_R = 1 << 8,
        DMF_L = 1 << 9,
        CM = 1 << 10,
        CM_L = 1 << 11,
        CM_R = 1 << 12,
        RMF = 1 << 13,
        LMF = 1 << 14,
        AMF = 1 << 15,
        AMF_R = 1 << 16,
        AMF_L = 1 << 17,
        LW = 1 << 18,
        RW = 1 << 19,
        ST = 1 << 20,
        ST_L = 1 << 21,
        ST_R = 1 << 22,
        ParametersCount = 1 << 23
    }
}
