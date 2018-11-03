create type BalTableType as table                                  
(Id int identity(1, 1), compcode int, mstbrnc int, trnitem int, trnledg int, sumopen money, sumdram money, sumcram money, sumbala money) 


CREATE NONCLUSTERED INDEX [ordemstIndex] ON ordemst ([compcode] ASC,[msttype] ASC,[mstcust] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [ordeitdIndex1] ON ordeitd ([itdtype] ASC, [itditem] asc)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [jourtrnIndex1] ON jourtrn ([trnitem] ASC,[trndate] Asc)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]


/*drop INDEX [jourmstIndex1] ON jourmst
CREATE NONCLUSTERED INDEX [jourmstIndex1] ON jourmst ([mstdate] ASC) INCLUDE (compcode, msttype, mstcode, mstbrnc) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

drop INDEX [jourtrnIndex2] ON jourtrn 
CREATE NONCLUSTERED INDEX [jourtrnIndex2] ON jourtrn ([trndram]) INCLUDE(compcode, trntype, trncode, trndate, trnsrno, trnitem, trnledg) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

drop INDEX [accountIndex1] ON account 
CREATE NONCLUSTERED INDEX [accountIndex1] ON account ([acctgrou]) INCLUDE(compcode, acctcode, acctname, acctcity, acctstat) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]


drop index [accountIndex2] ON account 
CREATE NONCLUSTERED INDEX [accountIndex2] ON account ([acctgrou]) INCLUDE(compcode, acctcode, acctname, acctcity, acctphon) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]*/
