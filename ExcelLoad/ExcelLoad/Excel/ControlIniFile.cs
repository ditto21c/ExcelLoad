/*
 * 
 * .ini 파일 관리 클래스
 * 
 * */

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

public class CControlIniFile
{
    public enum EControlIniFileRet
    {
        Succend = 0,                //!< 성공
        Failed_NotExistIniFile,     //!< Ini 파일이 존재 x
    };

    // 문자 쓰기
    [DllImport("kernel32")]
    private static extern bool WritePrivateProfileString(
  string strAppName_,
  string strKeyName_,
  string strString_,
  string strFileName_
    );

    // 문자 얻기
    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(
  string strAppName_,
  string strKeyName_,
  string strDefault_,
  StringBuilder strReturnedString_o,
  int nSize_,
  string strFileName_
        );



    // 숫자 얻기
    [DllImport("kernel32")]
    private static extern uint GetPrivateProfileInt(
  string strAppName_,
  string strKeyName_,
  int    nDefault,
  string strFileName_
    );

    string m_strFilePath;   //!< .ini 파일 기본 폴더 경로

    //!< .ini 파일 기본 폴더 경로 세팅
    private CControlIniFile(){}
    public CControlIniFile(string strFilePath_)
    {
        m_strFilePath = strFilePath_;
    }

    // 문자열 세팅
    public EControlIniFileRet WriteString(
        string strAppName_,
        string strKeyName_,
        string strString_,
        string strFileName_)
    {
        // 파일 검사
        strFileName_ = m_strFilePath + strFileName_;
        if (!existFile(strFileName_))
            return EControlIniFileRet.Failed_NotExistIniFile;

        EControlIniFileRet eRet = EControlIniFileRet.Succend;
        bool bRet = WritePrivateProfileString(strAppName_, strKeyName_, strString_, strFileName_);
        // 실패 이유 
        if (!bRet)
            eRet = EControlIniFileRet.Failed_NotExistIniFile;
        return eRet;
    }

    // 문자열 얻기
    public EControlIniFileRet GetString(
         string strAppName_,
         string strKeyName_,
         string strDefault_,
         out string strReturnedString_o,
         int nSize_,
         string strFileName_
    )
    {
        strFileName_ = m_strFilePath + strFileName_;
        if (!existFile(strFileName_))
        {
            strReturnedString_o = "";
            return EControlIniFileRet.Failed_NotExistIniFile;
        }

        EControlIniFileRet eRet = EControlIniFileRet.Succend;

        StringBuilder TempstrBuilder = new StringBuilder();
        int nRet = GetPrivateProfileString(strAppName_, strKeyName_, strDefault_, TempstrBuilder, nSize_, strFileName_);
        strReturnedString_o = TempstrBuilder.ToString();
        // 실패 이유
        if (nRet == 0)
            eRet = EControlIniFileRet.Failed_NotExistIniFile;
        
        return eRet;
    }

    //--------------------------------------------
    // 숫자 얻기
    // - 검색에 실패 했을시 nDefault_를 리턴한다.
    //--------------------------------------------
    public EControlIniFileRet GetInt(
        string strAppName_,
        string strKeyName_,
        int nDefault_,
        string strFileName_,
        out uint uValue_o
    )
    {
        strFileName_ = m_strFilePath + strFileName_;
        if (!existFile(strFileName_))
        {
            uValue_o = 0;
            return EControlIniFileRet.Failed_NotExistIniFile;
        }
        
        EControlIniFileRet eRet = EControlIniFileRet.Succend;
        uValue_o = GetPrivateProfileInt(strAppName_, strKeyName_, nDefault_, strFileName_);
        
        return eRet;
    }

    // 파일이 존재 하는지 체크
    bool existFile(string strFile_)
    {
        return File.Exists(strFile_);
    }

};