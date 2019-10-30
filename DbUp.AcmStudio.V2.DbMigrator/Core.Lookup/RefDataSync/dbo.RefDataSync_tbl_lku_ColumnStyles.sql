use ACM_Studio_V2
go

    truncate table dbo.tbl_lku_ColumnStyles

    merge into dbo.tbl_lku_ColumnStyles target
    using
    (
        values
            --begin --Geneva Column Properties   
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
            --end --Geneva Column Properties
               
             --begin --Merrill/Morgan Column Properties  
             --Merrill/Morgan New Assets Detail 
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 8, 'yyyy-MM-dd', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 9, 'yyyy-MM-dd', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 10, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 11, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 12, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 13, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 14, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 15, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 16, '#0.000000', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 34, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 35, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 36, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 37, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 38, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 39, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 40, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 41, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 42, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 47, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 48, '#,##0.00', 25),

            --Merrill/Morgan New Assets Summary
            ('BroadridgeUmaMerrillMorgan.NewAssetsSummary', 7, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsSummary', 8, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.NewAssetsSummary', 9, '#0.000000', 25),

            --Merrill/Morgan Pseudo Flows
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 8, 'yyyy-MM-dd', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 9, 'yyyy-MM-dd', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 10, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 11, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 12, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 13, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 14, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 15, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 16, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 17, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 18, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 19, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 20, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 21, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 22, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 23, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 24, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 25, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 26, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 27, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 28, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 40, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 41, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 42, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 43, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 44, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 45, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 46, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 47, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 48, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 50, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 51, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 52, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 53, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 54, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 55, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 56, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 57, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowDetail', 58, '#,##0.00', 25),

            ('BroadridgeUmaMerrillMorgan.PseudoflowSummary', 6, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowSummary', 7, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowSummary', 8, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowSummary', 9, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowSummary', 10, '#0.000000', 25),
            ('BroadridgeUmaMerrillMorgan.PseudoflowSummary', 11, '#0.000000', 25),
               
            --Merrill/Morgan On Going   
            ('BroadridgeMerrillMorgan.OngoingDetail', 8, 'yyyy-MM-dd', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 9, 'yyyy-MM-dd', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 10, '#,##0.00', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 11, '#,##0.00', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 12, '#,##0.00', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 13, '#,##0.00', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 14, '#,##0.00', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 15, '#,##0.00', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 16, '#,##0.00', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 17, '#,##0.00', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 18, '#,##0.00', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 19, '#,##0.00', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 20, '#,##0.00', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 21, '#,##0.00', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 22, '#,##0.00', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 23, '#,##0.00', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 24, '#,##0.00', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 25, '#,##0.00', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 26, '#0.0000000', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 27, '#0.0000000', 25),
            ('BroadridgeMerrillMorgan.OngoingDetail', 51, '#,##0.00', 25),  
            ('BroadridgeMerrillMorgan.OngoingDetail', 52, '#,##0.00', 25),

            ('BroadridgeUmaMerrillMorgan.OngoingSummary', 6, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.OngoingSummary', 7, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.OngoingSummary', 8, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.OngoingSummary', 9, '#,##0.00', 25),
            ('BroadridgeUmaMerrillMorgan.OngoingSummary', 10, '#0.000000', 25),
            ('BroadridgeUmaMerrillMorgan.OngoingSummary', 11, '#0.000000', 25),
            --end; --UMA Merrill/Morgan Column Properties   
               
               
            --begin --UMA Column Properties
            --UMA New Assets Detail 
            ('BroadridgeUma.NewAssetsDetail', 3, 'yyyy-MM-dd', 25),
            ('BroadridgeUma.NewAssetsDetail', 4, 'yyyy-MM-dd', 25),
            ('BroadridgeUma.NewAssetsDetail', 5, 'yyyy-MM-dd', 25),
            ('BroadridgeUma.NewAssetsDetail', 6, '#,##0.00', 25),
            ('BroadridgeUma.NewAssetsDetail', 7, '#,##0.00', 25),
            ('BroadridgeUma.NewAssetsDetail', 8, '#0.000000', 25),
            
            --UMA New Assets Summary
            ('BroadridgeUma.NewAssetsSummary', 8, '#,##0.00', 25),
            ('BroadridgeUma.NewAssetsSummary', 9, '#,##0.00', 25),
            ('BroadridgeUma.NewAssetsSummary', 10, '#0.000000', 25),
            --end ---UMA Column Properties   
               
        
            --UMA Ongoing Detail
            ('BroadridgeUma.OngoingDetail', 8, 'yyyy-MM-dd', 25),
            ('BroadridgeUma.OngoingDetail', 9, 'yyyy-MM-dd', 25),
            ('BroadridgeUma.OngoingDetail', 10, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 11, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 12, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 13, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 14, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 15, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 16, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 17, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 18, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 19, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 20, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 21, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 22, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 23, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 24, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 25, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 26, '#0.0000000', 25),
            ('BroadridgeUma.OngoingDetail', 27, '#0.0000000', 25),
            ('BroadridgeUma.OngoingDetail', 51, '#,##0.00', 25),
            ('BroadridgeUma.OngoingDetail', 52, '#,##0.00', 25),
            --end --UMA Ongoing Detail
        
            --UMA Ongoing Summary
            ('BroadridgeUma.OngoingSummary', 6, '#,##0.00', 25),
            ('BroadridgeUma.OngoingSummary', 7, '#,##0.00', 25),
            ('BroadridgeUma.OngoingSummary', 8, '#,##0.00', 25),
            ('BroadridgeUma.OngoingSummary', 9, '#,##0.00', 25),
            ('BroadridgeUma.OngoingSummary', 10, '#0.000000', 25),
            ('BroadridgeUma.OngoingSummary', 11, '#0.000000', 25),
            --end --UMA Ongoing Summary
        
            --UMA Pseudo Flow Detail
            ('BroadridgeUma.PseudoflowDetail', 8, 'yyyy-MM-dd', 25),
            ('BroadridgeUma.PseudoflowDetail', 9, 'yyyy-MM-dd', 25),
            ('BroadridgeUma.PseudoflowDetail', 10, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 11, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 12, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 13, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 14, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 15, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 16, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 17, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 18, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 19, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 20, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 21, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 22, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 23, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 24, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 25, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 26, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 27, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 28, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 40, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 41, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 42, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 43, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 44, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 45, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 46, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 47, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 48, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 50, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 51, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 52, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 53, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 54, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 55, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 56, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 57, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowDetail', 58, '#,##0.00', 25),

            ('BroadridgeUma.PseudoflowSummary', 6, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowSummary', 7, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowSummary', 8, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowSummary', 9, '#,##0.00', 25),
            ('BroadridgeUma.PseudoflowSummary', 10, '#0.000000', 25),
            ('BroadridgeUma.PseudoflowSummary', 11, '#0.000000', 25)
            --end --UMA Pseudo Flow Detail
            ------------------------------   


-- 
--             
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 8, 'yyyy-MM-dd', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 9, 'yyyy-MM-dd', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 10, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 11, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 12, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 13, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 14, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 15, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 16, '#0.000000', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 34, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 35, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 36, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 37, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 38, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 39, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 40, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 41, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 42, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 47, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.NewAssetsDetail', 48, '#,##0.00', 25),   
-- 
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 8, 'yyyy-MM-dd', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 9, 'yyyy-MM-dd', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 10, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 11, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 12, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 13, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 14, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 15, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 16, '#0.000000', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 34, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 35, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 36, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 37, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 38, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 39, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 40, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 41, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 42, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 47, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsDetail', 48, '#,##0.00', 25),
-- 
--             
-- 
--             ('BroadridgeUmaMerrillMorgan.OngoingSummary', 3, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.OngoingSummary', 4, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.OngoingSummary', 5, '#,##0.00', 25),
--             ('BroadridgeUmaMerrillMorgan.OngoingSummary', 6, '#0.000000', 25),
--             ('BroadridgeUmaMerrillMorgan.OngoingSummary', 7, '#,##0.00', 25),
-- 
--             ('BroadridgeUma.NewAssetsSummary', 9, '#,##0.00', 25),
--             ('BroadridgeUma.NewAssetsSummary', 10, '#0.000000', 25),
--             ('BroadridgeUma.NewAssetsSummary', 11, '#,##0.00', 25),
-- 
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsSummary', 9, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsSummary', 10, '#0.000000', 25),
--             ('BroadridgeUmaNonMerrillMorgan.NewAssetsSummary', 11, '#,##0.00', 25),
-- 
--             ('BroadridgeUma.OngoingDetail', 10, 'yyyy-MM-dd', 25),
--             ('BroadridgeUma.OngoingDetail', 11, '#,##0.00', 25),
--             ('BroadridgeUma.OngoingDetail', 12, '#,##0.00', 25),
--             ('BroadridgeUma.OngoingDetail', 13, '#,##0.00', 25),
--             ('BroadridgeUma.OngoingDetail', 14, '#0.0000000', 25),
--             ('BroadridgeUma.OngoingDetail', 15, '#0.0000000', 25),
--             ('BroadridgeUma.OngoingDetail', 16, '#,##0.00', 25),
-- 
--             ('BroadridgeUmaNonMerrillMorgan.OngoingDetail', 9, 'yyyy-MM-dd', 25),
--             ('BroadridgeUmaNonMerrillMorgan.OngoingDetail', 10, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.OngoingDetail', 11, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.OngoingDetail', 13, 'yyyy-MM-dd', 25),
--             ('BroadridgeUmaNonMerrillMorgan.OngoingDetail', 14, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.OngoingDetail', 15, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.OngoingDetail', 16, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.OngoingDetail', 17, '#0.0000000', 25),
--             ('BroadridgeUmaNonMerrillMorgan.OngoingDetail', 18, '#0.0000000', 25),
--             ('BroadridgeUmaNonMerrillMorgan.OngoingDetail', 19, '#,##0.00', 25),
-- 
--             ('BroadridgeUma.OngoingSummary', 3, '#,##0.00', 25),
--             ('BroadridgeUma.OngoingSummary', 4, '#,##0.00', 25),
--             ('BroadridgeUma.OngoingSummary', 5, '#,##0.00', 25),
--             ('BroadridgeUma.OngoingSummary', 6, '#0.000000', 25),
--             ('BroadridgeUma.OngoingSummary', 7, '#,##0.00', 25),
-- 
--             ('BroadridgeUmaNonMerrillMorgan.OngoingSummary', 3, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.OngoingSummary', 4, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.OngoingSummary', 5, '#,##0.00', 25),
--             ('BroadridgeUmaNonMerrillMorgan.OngoingSummary', 6, '#0.000000', 25),
--             ('BroadridgeUmaNonMerrillMorgan.OngoingSummary', 7, '#,##0.00', 25),
-- 
--             ('BroadridgeUma.PseudoflowDetail', 12, 'yyyy-MM-dd', 25),
--             ('BroadridgeUma.PseudoflowDetail', 13, '#,##0.00', 25),
--             ('BroadridgeUma.PseudoflowDetail', 14, '#,##0.00', 25),
--             ('BroadridgeUma.PseudoflowDetail', 15, '#,##0.00', 25),
--             ('BroadridgeUma.PseudoflowDetail', 16, '#,##0.00', 25),
--             ('BroadridgeUma.PseudoflowDetail', 17, '#,##0.00', 25),
--             ('BroadridgeUma.PseudoflowDetail', 18, '#,##0.00', 25),
--             ('BroadridgeUma.PseudoflowDetail', 19, '#,##0.00', 25),
--             ('BroadridgeUma.PseudoflowDetail', 20, '#,##0.00', 25),
--             ('BroadridgeUma.PseudoflowDetail', 21, '#0.00', 25),
--             ('BroadridgeUma.PseudoflowDetail', 22, '#0.00', 25),
--             ('BroadridgeUma.PseudoflowDetail', 23, '#0.00', 25),
--             ('BroadridgeUma.PseudoflowDetail', 24, '#0.000000', 25),
--             ('BroadridgeUma.PseudoflowDetail', 25, '#,##0.00', 25),
-- 
--             ('BroadridgeUma.PseudoflowSummary', 6, '#,##0.00', 25),
--             ('BroadridgeUma.PseudoflowSummary', 7, '#0.000000', 25),
--             ('BroadridgeUma.PseudoflowSummary', 8, '#,##0.00', 25),
-- 
--             ('UmaMerrill.NewAssetsDetail', 2, 'yyyy-MM-dd', 25),
--             ('UmaMerrill.NewAssetsDetail', 7, '#,##0.00', 25),
--             ('UmaMerrill.NewAssetsDetail', 8, '#0.000000', 25),
--             ('UmaMerrill.NewAssetsDetail', 9, '#,##0.00', 25),
--             ('UmaMerrill.NewAssetsDetail', 11, 'yyyy-MM-dd', 25),
-- 
--             ('UmaMerrill.NewAssetsDetail', 5, '#,##0.00', 25),
--             ('UmaMerrill.NewAssetsSummary', 6, '#0.000000', 25),
--             ('UmaMerrill.NewAssetsDetail', 7, '#,##0.00', 25),
-- 
--             ('UmaMerrill.OngoingDetail', 13, 'yyyy-MM-dd', 25),
--             ('UmaMerrill.OngoingDetail', 14, '#,##0.00', 25),
--             ('UmaMerrill.OngoingDetail', 15, '#,##0.00', 25),
--             ('UmaMerrill.OngoingDetail', 16, '#,##0.00', 25),
--             ('UmaMerrill.OngoingDetail', 17, '#0.000000', 25),
--             ('UmaMerrill.OngoingDetail', 18, '#0.000000', 25),
--             ('UmaMerrill.OngoingDetail', 19, '#,##0.00', 25)

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
