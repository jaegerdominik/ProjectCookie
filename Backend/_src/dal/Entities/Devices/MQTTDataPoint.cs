﻿using ProjectCookie._src.dal.Entities;

namespace DAL.Entities.Devices
{
    public class MQTTDataPoint : DataPoint
    {
        public string TopicName { get; set; }
    }
}