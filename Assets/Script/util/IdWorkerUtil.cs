using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class IdWorkerUtil
{
    //����ID
    private static long workerId;
    private static long twepoch = 1585644268888L; //Ψһʱ�䣬����һ�������ظ���������������趨��Ҫ���ڵ�ǰʱ���
    private static long sequence = 0L;
    private static int workerIdBits = 4; //�������ֽ�����4���ֽ��������������(����ΪLong���ͻ���֣����ƫ��64λ����������64λû������)
    public static long maxWorkerId = -1L ^ -1L << workerIdBits; //������ID
    private static int sequenceBits = 10; //�������ֽ�����10���ֽ��������������
    private static int workerIdShift = sequenceBits; //��������������λ�������Ǻ��������ռ�õ�λ��
    private static int timestampLeftShift = sequenceBits + workerIdBits; //ʱ������ƶ�λ�����ǻ�����ͼ��������ֽ���
    public static long sequenceMask = -1L ^ -1L << sequenceBits; //һ΢���ڿ��Բ�������������ﵽ��ֵ��ȵ���һ΢���ڽ�������
    private long lastTimestamp = -1L;

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="workerId"></param>
    public IdWorkerUtil(long workerId)
    {
        if (workerId > maxWorkerId || workerId < 0)
            throw new Exception(string.Format("worker Id can't be greater than {0} or less than 0 ", workerId));
        IdWorkerUtil.workerId = workerId;
    }

    public long nextId()
    {
        lock (this)
        {
            long timestamp = timeGen();
            if (this.lastTimestamp == timestamp)
            { //ͬһ΢��������ID
                IdWorkerUtil.sequence = (IdWorkerUtil.sequence + 1) & IdWorkerUtil.sequenceMask; //��&��������΢���ڲ����ļ����Ƿ��Ѿ���������
                if (IdWorkerUtil.sequence == 0)
                {
                    //һ΢���ڲ�����ID�����Ѵ����ޣ��ȴ���һ΢��
                    timestamp = tillNextMillis(this.lastTimestamp);
                }
            }
            else
            { //��ͬ΢������ID
                IdWorkerUtil.sequence = 0; //������0
            }
            if (timestamp < lastTimestamp)
            { //�����ǰʱ�������һ������IDʱʱ�����С���׳��쳣����Ϊ���ܱ�֤�������ɵ�ID֮ǰû�����ɹ�
                throw new Exception(string.Format("Clock moved backwards.  Refusing to generate id for {0} milliseconds",
                    this.lastTimestamp - timestamp));
            }
            this.lastTimestamp = timestamp; //�ѵ�ǰʱ�������Ϊ�������ID��ʱ���
            long nextId = 
                (timestamp - twepoch << timestampLeftShift) | IdWorkerUtil.workerId << IdWorkerUtil.workerIdShift | IdWorkerUtil.sequence;
            return nextId;
        }
    }

    /// <summary>
    /// ��ȡ��һ΢��ʱ���
    /// </summary>
    /// <param name="lastTimestamp"></param>
    /// <returns></returns>
    private long tillNextMillis(long lastTimestamp)
    {
        long timestamp = timeGen();
        while (timestamp <= lastTimestamp)
        {
            timestamp = timeGen();
        }
        return timestamp;
    }

    /// <summary>
    /// ���ɵ�ǰʱ���
    /// </summary>
    /// <returns></returns>
    private long timeGen()
    {
        return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
    }
}

/// <summary>
/// ����ѩ��ID
/// </summary>
public static class SnowFlake
{
    private static long _workerId = 9;
    private static IdWorkerUtil _idWorker = null;

    public static string NewId()
    {
        if (_idWorker == null)
            _idWorker = new IdWorkerUtil(_workerId);
        return _idWorker.nextId().ToString();
    }
}