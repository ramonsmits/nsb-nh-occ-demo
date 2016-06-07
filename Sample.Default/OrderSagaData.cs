﻿using System;
using NServiceBus.Saga;
using NServiceBus.SagaPersisters.NHibernate;

[LockMode(LockModes.Read)]
public class OrderSagaData : IContainSagaData
{
    public virtual Guid Id { get; set; }
    public virtual string OriginalMessageId { get; set; }
    public virtual string Originator { get; set; }
    [Unique]
    public virtual string OrderId { get; set; }
    [RowVersion]
    public virtual int Version { get; set; }
    public virtual int Count { get; set; }
}

