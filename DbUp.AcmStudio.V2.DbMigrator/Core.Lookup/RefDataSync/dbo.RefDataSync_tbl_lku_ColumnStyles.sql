use ACM_Studio_V2
go

    truncate table dbo.tbl_lku_ColumnStyles

    merge into dbo.tbl_lku_ColumnStyles target
    using
    (
        values
            ('GevevaSma.NewAssetsDetail', 8, 'YYYY-mm-dd', 25),
			('GevevaSma.NewAssetsDetail', 9, 'YYYY-mm-dd', 25),
            ('GevevaSma.NewAssetsDetail', 13, '#,##0.00', 25),
            ('GevevaSma.NewAssetsDetail', 14, '#,##0.00', 25),
            ('GevevaSma.NewAssetsDetail', 15, '#,##0.00', 25),
            ('GevevaSma.NewAssetsDetail', 16, '#0.000000', 25),
            ('GevevaSma.NewAssetsDetail', 17, '#0.000000', 25),

            ('GevevaSma.NewAssetsSummary', 1, '', 25),
            ('GevevaSma.NewAssetsSummary', 2, '', 50),
            ('GevevaSma.NewAssetsSummary', 3, '', 50),
            ('GevevaSma.NewAssetsSummary', 4, '#,##0.00', 25),
            ('GevevaSma.NewAssetsSummary', 5, '#,##0.00', 25),
            ('GevevaSma.NewAssetsSummary', 6, '#0.000000', 25),

            ('GevevaSma.OngoingDetail', 4, 'YYYY-mm-dd', 25),
            ('GevevaSma.OngoingDetail', 8, 'YYYY-mm-dd', 25),
            ('GevevaSma.OngoingDetail', 9, '#,##0.00', 25),
            ('GevevaSma.OngoingDetail', 10, '#,##0.00', 25),
            ('GevevaSma.OngoingDetail', 11, '#,##0.00', 25),
            ('GevevaSma.OngoingDetail', 12, '#0.000000', 25),
            ('GevevaSma.OngoingDetail', 13, '#0.000000', 25),

            ('GevevaSma.OngoingSummary', 1, '', 25),
            ('GevevaSma.OngoingSummary', 2, '', 50),
            ('GevevaSma.OngoingSummary', 3, '', 50),
            ('GevevaSma.OngoingSummary', 4, '#,##0.00', 25),
            ('GevevaSma.OngoingSummary', 5, '#,##0.00', 25),
            ('GevevaSma.OngoingSummary', 6, '#,##0.00', 25),
            ('GevevaSma.OngoingSummary', 7, '#,##0.00', 25),
            ('GevevaSma.OngoingSummary', 8, '#0.000000', 25),
            ('GevevaSma.OngoingSummary', 9, '#0.0000##', 25),

            ('BroadridgeUma.NewAssetsDetail', 2, 'yyyy-MM-dd', 25),
            ('BroadridgeUma.NewAssetsDetail', 11, '#,##0.00', 25),
            ('BroadridgeUma.NewAssetsDetail', 14, '#,##0.00', 25),
            ('BroadridgeUma.NewAssetsDetail', 15, '#0.000000', 25),
            ('BroadridgeUma.NewAssetsDetail', 16, '#,##0.00', 25),

            ('BroadridgeUma.NewAssetsSummary', 9, '#,##0.00', 25),
            ('BroadridgeUma.NewAssetsSummary', 10, '#0.000000', 25),
            ('BroadridgeUma.NewAssetsSummary', 11, '#,##0.00', 25),

            ('BroadridgeUma.OngoingDetail', 9, 'yyyy-MM-dd', 25),
            ('BroadridgeUma.OngoingDetail', 10, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 11, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 12, 'yyyy-MM-dd', 25),
            ('BroadridgeUma.OngoingDetail', 13, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 14, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 15, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 16, '#0.0000000', 25),
            ('BroadridgeUma.OngoingDetail', 17, '#0.0000000', 25),
            ('BroadridgeUma.OngoingDetail', 18, '#,##0.00', 25),

            ('BroadridgeUma.OngoingSummary', 3, '#,##0.00', 25),
            ('BroadridgeUma.OngoingSummary', 4, '#,##0.00', 25),
            ('BroadridgeUma.OngoingSummary', 5, '#,##0.00', 25),
            ('BroadridgeUma.OngoingSummary', 6, '#0.000000', 25),
            ('BroadridgeUma.OngoingSummary', 7, '#,##0.00', 25),

            ('BroadridgeUma.PseudoflowDetail', 12, 'yyyy-MM-dd', 25),
            ('BroadridgeUma.PseudoflowDetail', 13, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 14, '#0.000000', 25),
            ('BroadridgeUma.PseudoflowDetail', 15, '#,##0.00', 25),

            ('BroadridgeUma.PseudoflowSummary', 3, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowSummary', 4, '#0.000000', 25),
            ('BroadridgeUma.PseudoflowSummary', 5, '#,##0.00', 25),

            ('UmaMerrill.NewAssetsDetail', 2, 'yyyy-MM-dd', 25),
            ('UmaMerrill.NewAssetsDetail', 7, '#,##0.00', 25),
            ('UmaMerrill.NewAssetsDetail', 8, '#0.000000', 25),
            ('UmaMerrill.NewAssetsDetail', 9, '#,##0.00', 25),
            ('UmaMerrill.NewAssetsDetail', 11, 'yyyy-MM-dd', 25),

            ('UmaMerrill.NewAssetsDetail', 5, '#,##0.00', 25),
            ('UmaMerrill.NewAssetsSummary', 6, '#0.000000', 25),
            ('UmaMerrill.NewAssetsDetail', 7, '#,##0.00', 25),

            ('UmaMerrill.OngoingDetail', 8, '#,##0.00', 25),
            ('UmaMerrill.OngoingDetail', 9, '#,##0.00', 25),
            ('UmaMerrill.OngoingDetail', 10, '#0.000000', 25),
            ('UmaMerrill.OngoingDetail', 11, '#0.000000', 25),
            ('UmaMerrill.OngoingDetail', 12, '#,##0.00', 25),
            ('UmaMerrill.OngoingDetail', 13, '#,##0.00', 25),
            ('UmaMerrill.OngoingDetail', 14, 'yyyy-MM-dd', 25),
            ('UmaMerrill.OngoingDetail', 15, 'yyyy-MM-dd', 25)

    ) as source
    (
        ReportName,
        ColumnNumber,
        FormatString,
        ColumnWidth
    )
    on (source.ReportName = target.ReportName)

    when matched
        and source.ColumnNumber <> target.ColumnNumber
        or source.FormatString  <> target.FormatString
        or source.ColumnWidth   <> target.ColumnWidth
    then update set
        target.ColumnNumber     = source.ColumnNumber,
        target.FormatString     = source.FormatString,
        target.ColumnWidth      = source.ColumnWidth
    when not matched then
        insert
            (
              ReportName,
              ColumnNumber,
              FormatString,
              ColumnWidth
            )
        values
            (
              source.ReportName,
              source.ColumnNumber,
              source.FormatString,
              source.ColumnWidth
            )

    when not matched by source then
        delete;