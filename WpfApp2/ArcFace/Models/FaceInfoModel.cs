using System;
using System.Collections.Generic;
using System.Text;

namespace AsfFace.Models
{
    /// <summary>
    /// 人脸特征信息Model类
    /// 作者:西瓜码农 博客:https://www.xgblog.cn
    /// </summary>
    public class FaceInfoModel
    {
        /// <summary>
        /// 年龄
        /// </summary>
        public int age { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int gender { get; set; }

        /// <summary>
        /// 3D角度信息
        /// </summary>
        public Face3DAngleModel face3dAngle { get; set; }

        /// <summary>
        /// 人脸框信息
        /// </summary>
        public AsfStruct.ASF_FaceRect faceRect { get; set; }
    }
}
