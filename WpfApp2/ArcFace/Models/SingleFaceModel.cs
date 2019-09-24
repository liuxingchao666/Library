using System.Drawing;

namespace AsfFace.Models
{
    /// <summary>
    /// 单人脸信息Model类
    /// 作者:西瓜码农 博客:https://www.xgblog.cn
    /// </summary>
    public class SingleFaceModel
    {
       
        /// <summary>
        /// 人脸框
        /// </summary>
        public Rectangle FaceRect { get; set; }

        /// <summary>
        /// 人相角度
        /// </summary>
        public int FaceOrient { get; set; }


        public SingleFaceModel(){}

        public SingleFaceModel(AsfStruct.ASF_SingleFaceInfo singleFaceInfo)
        {
            this.FaceRect = singleFaceInfo.faceRect.GetRectangle();
            this.FaceOrient = singleFaceInfo.faceOrient;
        }

    }

}
