using System;

namespace nethessian
{
	/// <summary>
	/// ErrorItem �̊T�v�̐����ł��B
	/// </summary>
	public class ErrorItem
	{
		public ErrorItem()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
		public String cat; // E:Error W:Warning
		public String errElement;
		public String errMsg;// EXXX-Error Message
		public String errData;
	}
}
