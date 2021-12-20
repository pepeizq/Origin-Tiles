Module OriginBBDD

    'https://api3.origin.com/supercat/US/en_US/supercat-PCWIN_MAC-US-en_US.json.gz
    'https://api3.origin.com/supercat/GB/en_GB/supercat-PCWIN_MAC-GB-en_GB.json.gz

    Public Function BuscarenListado(idOrigin As String, devolver As Integer)

        Dim lista As New List(Of OriginBBDDEntrada) From {
            New OriginBBDDEntrada("A Way Out", New List(Of String) From {
                "Origin.OFR.50.0001900", "Origin.OFR.50.0002487", "Origin.OFR.50.0002488"}, "1222700"),
            New OriginBBDDEntrada("Apex Legends", New List(Of String) From {"Origin.OFR.50.0002694"}, "1172470"),
            New OriginBBDDEntrada("Anthem", New List(Of String) From {
                "Origin.OFR.50.0001521", "Origin.OFR.50.0002688", "Origin.OFR.50.0002755", "Origin.OFR.50.0002760",
                "Origin.OFR.50.0002980"}, Nothing),
            New OriginBBDDEntrada("Battlefield 1", New List(Of String) From {
                "Origin.OFR.50.0000557", "Origin.OFR.50.0001380", "Origin.OFR.50.0001381", "Origin.OFR.50.0002321",
                "Origin.OFR.50.0002323", "Origin.OFR.50.0002324"}, "1238840"),
            New OriginBBDDEntrada("Battlefield 1942", New List(Of String) From {"OFB-EAST:56186"}, Nothing),
            New OriginBBDDEntrada("Battlefield 2 Complete Collection", New List(Of String) From {"DR:78869400"}, Nothing),
            New OriginBBDDEntrada("Battlefield 2042", New List(Of String) From {
                "Origin.OFR.50.0004152", "Origin.OFR.50.0004491", "Origin.OFR.50.0004492", "Origin.OFR.50.0004494",
                "Origin.OFR.50.0004495", "Origin.OFR.50.0004497", "Origin.OFR.50.0004507"}, "1517290"),
            New OriginBBDDEntrada("Battlefield 3", New List(Of String) From {"DR:225064100", "DR:224766400"}, "1238820"),
            New OriginBBDDEntrada("Battlefield 4", New List(Of String) From {
                "OFB-EAST:109552316", "OFB-EAST:109546867", "OFB-EAST:109549060", "OFB-EAST:109552154",
                "OFB-EAST:109552155", "OFB-EAST:109552312", "OFB-EAST:109552314"}, "1238860"),
            New OriginBBDDEntrada("Battlefield Bad Company 2", New List(Of String) From {"DR:156691300"}, "24960"),
            New OriginBBDDEntrada("Battlefield Hardline", New List(Of String) From {
                "Origin.OFR.50.0000426", "Origin.OFR.50.0000846"}, "1238880"),
            New OriginBBDDEntrada("Battlefield V", New List(Of String) From {
                "Origin.OFR.50.0002683", "Origin.OFR.50.0002859", "Origin.OFR.50.0002860", "Origin.OFR.50.0002861",
                "Origin.OFR.50.0002862", "Origin.OFR.50.0002863", "Origin.OFR.50.0002942", "Origin.OFR.50.0004340",
                "Origin.OFR.50.0004343", "Origin.OFR.50.0004346"}, "1238810"),
            New OriginBBDDEntrada("Bejeweled 3", New List(Of String) From {"OFB-EAST:49753"}, "78000"),
            New OriginBBDDEntrada("Bejeweled Twist", New List(Of String) From {"OFB-EAST:109550788"}, "3560"),
            New OriginBBDDEntrada("Big Money", New List(Of String) From {"OFB-EAST:109552136"}, "3360"),
            New OriginBBDDEntrada("Burnout Paradise Remastered", New List(Of String) From {
                "Origin.OFR.50.0002535", "Origin.OFR.50.0002537", "Origin.OFR.50.0002541"}, "1238080"),
            New OriginBBDDEntrada("Burnout Paradise The Ultimate Box", New List(Of String) From {"DR:106479900"}, "24740"),
            New OriginBBDDEntrada("Command & Conquer", New List(Of String) From {"OFB-EAST:52016"}, Nothing),
            New OriginBBDDEntrada("Command & Conquer 3: Tiberium Wars", New List(Of String) From {"OFB-EAST:52210"}, "24790"),
            New OriginBBDDEntrada("Command & Conquer 4: Tiberian Twilight", New List(Of String) From {"OFB-EAST:52212"}, "47700"),
            New OriginBBDDEntrada("Command & Conquer Generals", New List(Of String) From {"OFB-EAST:52209"}, Nothing),
            New OriginBBDDEntrada("Command & Conquer Red Alert", New List(Of String) From {"OFB-EAST:52017"}, Nothing),
            New OriginBBDDEntrada("Command & Conquer Red Alert 2", New List(Of String) From {"OFB-EAST:52019"}, Nothing),
            New OriginBBDDEntrada("Command & Conquer Red Alert 3", New List(Of String) From {
                "OFB-EAST:52211", "DR:106608500"}, "17480"),
            New OriginBBDDEntrada("Command & Conquer Red Alert 3 Uprising", New List(Of String) From {
                "DR:111298000", "DR:110709700"}, "24800"),
            New OriginBBDDEntrada("Command & Conquer Remastered Collection", New List(Of String) From {
                "Origin.OFR.50.0003515", "Origin.OFR.50.0004022", "Origin.OFR.50.0004044"}, "1213210"),
            New OriginBBDDEntrada("Command & Conquer Renegade", New List(Of String) From {"OFB-EAST:52208"}, Nothing),
            New OriginBBDDEntrada("Command & Conquer Tiberian Sun", New List(Of String) From {"OFB-EAST:52018"}, Nothing),
            New OriginBBDDEntrada("Crusader: No Regret", New List(Of String) From {"OFB-EAST:39132"}, Nothing),
            New OriginBBDDEntrada("Crusader: No Remorse", New List(Of String) From {"DR:231812900"}, Nothing),
            New OriginBBDDEntrada("Crysis", New List(Of String) From {"DR:77033800", "OFB-EAST:57289"}, "17300"),
            New OriginBBDDEntrada("Crysis 2", New List(Of String) From {"OFB-EAST:49464"}, "108800"),
            New OriginBBDDEntrada("Crysis 3", New List(Of String) From {
                "OFB-EAST:49562", "OFB-EAST:49459", "OFB-EAST:49744", "Origin.OFR.50.0001095", "Origin.OFR.50.0001098"}, "1282690"),
            New OriginBBDDEntrada("Crysis Warhead", New List(Of String) From {"DR:105905500"}, "17330"),
            New OriginBBDDEntrada("Dead Space", New List(Of String) From {"OFB-EAST:109547518"}, "17470"),
            New OriginBBDDEntrada("Dead Space 2", New List(Of String) From {"OFB-EAST:55619"}, "47780"),
            New OriginBBDDEntrada("Dead Space 3", New List(Of String) From {"OFB-EAST:50885", "OFB-EAST:55281"}, "1238060"),
            New OriginBBDDEntrada("Dragon Age II", New List(Of String) From {"DR:201797000", "OFB-EAST:59474"}, "1238040"),
            New OriginBBDDEntrada("Dragon Age Inquisition", New List(Of String) From {
                "Origin.OFR.50.0001131", "Origin.OFR.50.0001132", "OFB-EAST:1000026", "OFB-EAST:1000030"}, "1222690"),
            New OriginBBDDEntrada("Dragon Age Origins", New List(Of String) From {
                "DR:208591800", "OFB-EAST:51582", "Origin.OFR.50.0001535", "Origin.OFR.50.0001540"}, "47810"),
            New OriginBBDDEntrada("Dungeon Keeper", New List(Of String) From {"DR:231813000"}, Nothing),
            New OriginBBDDEntrada("Dungeon Keeper 2", New List(Of String) From {"OFB-EAST:39131"}, Nothing),
            New OriginBBDDEntrada("Fe", New List(Of String) From {"Origin.OFR.50.0001944", "Origin.OFR.50.0002497"}, "1225580"),
            New OriginBBDDEntrada("Feeding Frenzy 2: Shipwreck Showdown", New List(Of String) From {"OFB-EAST:109552142"}, "3390"),
            New OriginBBDDEntrada("FIFA 20", New List(Of String) From {
                "Origin.OFR.50.0003156", "Origin.OFR.50.0003453", "Origin.OFR.50.0003640"}, Nothing),
            New OriginBBDDEntrada("FIFA 21", New List(Of String) From {
                "Origin.OFR.50.0003912", "Origin.OFR.50.0003917", "Origin.OFR.50.0004184", "Origin.OFR.50.0004195",
                "Origin.OFR.50.0004215", "Origin.OFR.50.0004224", "Origin.OFR.50.0004225", "Origin.OFR.50.0004235",
                "Origin.OFR.50.0004237"}, "1313860"),
            New OriginBBDDEntrada("FIFA 22", New List(Of String) From {
                "Origin.OFR.50.0004526", "Origin.OFR.50.0004566", "Origin.OFR.50.0004567", "Origin.OFR.50.0004569",
                "Origin.OFR.50.0004613", "Origin.OFR.50.0004615", "Origin.OFR.50.0004669", "Origin.OFR.50.0004682"}, "1506830"),
            New OriginBBDDEntrada("Jade Empire: Special Edition", New List(Of String) From {"OFB-EAST:1000017"}, "7110"),
            New OriginBBDDEntrada("Knockout City", New List(Of String) From {
                "Origin.OFR.50.0003487", "Origin.OFR.50.0003985", "Origin.OFR.50.0004179", "Origin.OFR.50.0004455",
                "Origin.OFR.50.0004457"}, "1301210"),
            New OriginBBDDEntrada("It Takes Two", New List(Of String) From {
                "Origin.OFR.50.0004024", "Origin.OFR.50.0004288", "Origin.OFR.50.0004322"}, "1426210"),
            New OriginBBDDEntrada("Lost in Random", New List(Of String) From {
                "Origin.OFR.50.0004316", "Origin.OFR.50.0004317"}, "1462570"),
            New OriginBBDDEntrada("Madden NFL 20", New List(Of String) From {
                "Origin.OFR.50.0003091", "Origin.OFR.50.0003417"}, Nothing),
            New OriginBBDDEntrada("Madden NFL 21", New List(Of String) From {
                "Origin.OFR.50.0003744", "Origin.OFR.50.0004029", "Origin.OFR.50.0004122", "Origin.OFR.50.0004123",
                "Origin.OFR.50.0004124", "Origin.OFR.50.0004125", "Origin.OFR.50.0004415"}, "1239520"),
            New OriginBBDDEntrada("Madden NFL 22", New List(Of String) From {
                "Origin.OFR.50.0004429", "Origin.OFR.50.0004436", "Origin.OFR.50.0004617", "Origin.OFR.50.0004633",
                "Origin.OFR.50.0004643"}, "1519350"),
            New OriginBBDDEntrada("Magic Carpet", New List(Of String) From {"DR:231813100"}, Nothing),
            New OriginBBDDEntrada("Mass Effect", New List(Of String) From {"DR:102427200", "OFB-EAST:55465"}, "17460"),
            New OriginBBDDEntrada("Mass Effect 2", New List(Of String) From {"OFB-EAST:56694", "Origin.OFR.50.0001377"}, "24980"),
            New OriginBBDDEntrada("Mass Effect 3", New List(Of String) From {
                "DR:229644400", "DR:230773600", "OFB-EAST:43437", "OFB-EAST:43438"}, "1238020"),
            New OriginBBDDEntrada("Mass Effect Andromeda", New List(Of String) From {
                "Origin.OFR.50.0001536", "Origin.OFR.50.0001539", "Origin.OFR.50.0001646", "Origin.OFR.50.0001649"}, "1238000"),
            New OriginBBDDEntrada("Mass Effect Legendary Edition", New List(Of String) From {
                "Origin.OFR.50.0004049", "Origin.OFR.50.0004264", "Origin.OFR.50.0004291"}, "1328670"),
            New OriginBBDDEntrada("Medal of Honor", New List(Of String) From {"DR:215619100"}, "24840"),
            New OriginBBDDEntrada("Medal of Honor: Airbone", New List(Of String) From {"DR:80586700"}, "47790"),
            New OriginBBDDEntrada("Medal of Honor: Allied Assault", New List(Of String) From {"OFB-EAST:1000011"}, Nothing),
            New OriginBBDDEntrada("Medal of Honor: Pacific Assault", New List(Of String) From {"Origin.OFR.50.0000357"}, Nothing),
            New OriginBBDDEntrada("Medal of Honor: Warfighter", New List(Of String) From {"OFB-EAST:46113", "OFB-EAST:50884"}, Nothing),
            New OriginBBDDEntrada("Mirror's Edge", New List(Of String) From {"DR:106999100", "Origin.OFR.50.0001843"}, "17410"),
            New OriginBBDDEntrada("Mirror's Edge Catalyst", New List(Of String) From {
                "Origin.OFR.50.0001000", "Origin.OFR.50.0000998"}, "1233570"),
            New OriginBBDDEntrada("Need For Speed", New List(Of String) From {
                "Origin.OFR.50.0000994", "Origin.OFR.50.0000995", "Origin.OFR.50.0001006", "Origin.OFR.50.0001009",
                "Origin.OFR.50.0001011"}, "1262540"),
            New OriginBBDDEntrada("Need For Speed Heat", New List(Of String) From {
                "Origin.OFR.50.0003424", "Origin.OFR.50.0003426", "Origin.OFR.50.0003439", "Origin.OFR.50.0003440",
                "Origin.OFR.50.0003514"}, "1222680"),
            New OriginBBDDEntrada("Need For Speed Hot Pursuit", New List(Of String) From {"DR:210703600"}, Nothing),
            New OriginBBDDEntrada("Need For Speed Hot Pursuit Remastered", New List(Of String) From {
                "Origin.OFR.50.0004030", "Origin.OFR.50.0004092", "Origin.OFR.50.0004310"}, "1328660"),
            New OriginBBDDEntrada("Need For Speed Most Wanted", New List(Of String) From {
                "OFB-EAST:46851", "OFB-EAST:52699"}, "1262560"),
            New OriginBBDDEntrada("Need For Speed Payback", New List(Of String) From {
                "Origin.OFR.50.0001684", "Origin.OFR.50.0002149", "Origin.OFR.50.0002155", "Origin.OFR.50.0002167",
                "Origin.OFR.50.0002168", "Origin.OFR.50.0002170"}, "1262580"),
            New OriginBBDDEntrada("Need For Speed Rivals", New List(Of String) From {
                "Origin.OFR.50.0000676", "Origin.OFR.50.0000677", "OFB-EAST:109550809", "OFB-EAST:56522"}, "1262600"),
            New OriginBBDDEntrada("Need For Speed Shift 2 Unleashed", New List(Of String) From {"DR:224563400"}, "47920"),
            New OriginBBDDEntrada("Need For Speed The Run", New List(Of String) From {"DR:231088400"}, Nothing),
            New OriginBBDDEntrada("Nox", New List(Of String) From {"DR:235663500"}, Nothing),
            New OriginBBDDEntrada("Peggle Deluxe", New List(Of String) From {"Origin.OFR.50.0000461"}, "3480"),
            New OriginBBDDEntrada("Plants vs. Zombies: Battle for Neighborville", New List(Of String) From {
                "Origin.OFR.50.0002623", "Origin.OFR.50.0003568", "Origin.OFR.50.0003634", "Origin.OFR.50.0003673",
                "Origin.OFR.50.0003675", "Origin.OFR.50.0004019", "Origin.OFR.50.0004020"}, "1262240"),
            New OriginBBDDEntrada("Plants vs. Zombies: Game of the Year", New List(Of String) From {"OFB-EAST:48217"}, "3590"),
            New OriginBBDDEntrada("Plants vs. Zombies: Garden Warfare 2", New List(Of String) From {"Origin.OFR.50.0000786", "Origin.OFR.50.0001051"}, Nothing),
            New OriginBBDDEntrada("Rocket Arena", New List(Of String) From {
                "Origin.OFR.50.0003997", "Origin.OFR.50.0004026", "Origin.OFR.50.0004162", "Origin.OFR.50.0004169",
                "Origin.OFR.50.0004171"}, "1233550"),
            New OriginBBDDEntrada("Sea of Solitude", New List(Of String) From {"Origin.OFR.50.0002405"}, "1225590"),
            New OriginBBDDEntrada("Shank", New List(Of String) From {"DR:224434500"}, "6120"),
            New OriginBBDDEntrada("Sid Meier's Alpha Centauri", New List(Of String) From {"DR:231813200"}, Nothing),
            New OriginBBDDEntrada("SimCity 2000 Special Edition", New List(Of String) From {"DR:235664600"}, Nothing),
            New OriginBBDDEntrada("SimCity 4 Deluxe Edition", New List(Of String) From {"DR:198095500"}, "24780"),
            New OriginBBDDEntrada("SimCity", New List(Of String) From {
                "OFB-EAST:48205", "Origin.OFR.50.0000029", "Origin.OFR.50.0000741"}, Nothing),
            New OriginBBDDEntrada("Spore", New List(Of String) From {"DR:91619200"}, "17390"),
            New OriginBBDDEntrada("Star Wars Battlefront", New List(Of String) From {"Origin.OFR.50.0001211"}, "1237980"),
            New OriginBBDDEntrada("Star Wars Battlefront II", New List(Of String) From {
                "Origin.OFR.50.0001523", "Origin.OFR.50.0002015", "Origin.OFR.50.0002148", "Origin.OFR.50.0002898"}, "1237950"),
            New OriginBBDDEntrada("Star Wars Jedi: Fallen Order", New List(Of String) From {
                "Origin.OFR.50.0003794", "Origin.OFR.50.0003795", "Origin.OFR.50.0003797", "Origin.OFR.50.0003798",
                "Origin.OFR.50.0003815", "Origin.OFR.50.0004032"}, "1172380"),
            New OriginBBDDEntrada("Star Wars Squadrons", New List(Of String) From {
                "Origin.OFR.50.0003112", "Origin.OFR.50.0003896", "Origin.OFR.50.0004097", "Origin.OFR.50.0004099",
                "Origin.OFR.50.0004214"}, "1222730"),
            New OriginBBDDEntrada("Star Wars The Old Republic", New List(Of String) From {
                "DR:231255400", "DR:231240400", "Origin.OFR.50.0000006"}, "1286830"),
            New OriginBBDDEntrada("Syndicate (1993)", New List(Of String) From {"OFB-EAST:60531"}, Nothing),
            New OriginBBDDEntrada("Syndicate (2012)", New List(Of String) From {"DR:235934200"}, Nothing),
            New OriginBBDDEntrada("The Saboteur", New List(Of String) From {"DR:106999600"}, Nothing),
            New OriginBBDDEntrada("The Sims 3", New List(Of String) From {"OFB-EAST:55107"}, "47890"),
            New OriginBBDDEntrada("The Sims 4", New List(Of String) From {
                "OFB-EAST:109552299", "OFB-EAST:109552408", "OFB-EAST:109552410", "OFB-EAST:109552414"}, "1222670"),
            New OriginBBDDEntrada("Theme Hospital", New List(Of String) From {"Origin.OFR.50.0000500"}, Nothing),
            New OriginBBDDEntrada("Titanfall Deluxe Edition", New List(Of String) From {"Origin.OFR.50.0000739"}, "1454890"),
            New OriginBBDDEntrada("Titanfall 2", New List(Of String) From {
                "Origin.OFR.50.0001451", "Origin.OFR.50.0001452", "Origin.OFR.50.0001455", "Origin.OFR.50.0001456"}, "1237970"),
            New OriginBBDDEntrada("Ultima I: The First Age of Darkness", New List(Of String) From {"OFB-EAST:39130"}, Nothing),
            New OriginBBDDEntrada("Ultima II: The Revenge of the Enchantress", New List(Of String) From {"OFB-EAST:39464"}, Nothing),
            New OriginBBDDEntrada("Ultima III: Exodus", New List(Of String) From {"OFB-EAST:39465"}, Nothing),
            New OriginBBDDEntrada("Ultima IV: Quest of the Avatar", New List(Of String) From {"OFB-EAST:39466"}, Nothing),
            New OriginBBDDEntrada("Ultima V: Warriors of Destiny", New List(Of String) From {"OFB-EAST:39467"}, Nothing),
            New OriginBBDDEntrada("Ultima VI: The False Prophet", New List(Of String) From {"OFB-EAST:39468"}, Nothing),
            New OriginBBDDEntrada("Ultima VII: The Complete Edition", New List(Of String) From {"OFB-EAST:39470"}, Nothing),
            New OriginBBDDEntrada("Ultima VIII: Gold Edition", New List(Of String) From {"OFB-EAST:39471"}, Nothing),
            New OriginBBDDEntrada("Ultima IX: Ascension", New List(Of String) From {"OFB-EAST:39469"}, Nothing),
            New OriginBBDDEntrada("Unravel", New List(Of String) From {"Origin.OFR.50.0000823"}, "1225560"),
            New OriginBBDDEntrada("Unravel Two", New List(Of String) From {
                "Origin.OFR.50.0002403", "Origin.OFR.50.0002520", "Origin.OFR.50.0002522"}, "1225570"),
            New OriginBBDDEntrada("Wing Commander", New List(Of String) From {"OFB-EAST:39133"}, Nothing),
            New OriginBBDDEntrada("Wing Commander II: Vengeance of the Kilrathi", New List(Of String) From {"OFB-EAST:39135"}, Nothing),
            New OriginBBDDEntrada("Wing Commander III: Heart of the Tiger", New List(Of String) From {"DR:235664700"}, Nothing),
            New OriginBBDDEntrada("Wing Commander IV: The Price of Freedom", New List(Of String) From {"OFB-EAST:39460"}, Nothing),
            New OriginBBDDEntrada("Wing Commander: Privateer", New List(Of String) From {"DR:231813400"}, Nothing),
            New OriginBBDDEntrada("Zuma's Revenge", New List(Of String) From {"OFB-EAST:52735"}, "3620")
        }

        If devolver = 0 Then
            Dim idSteam As String = String.Empty

            For Each juego In lista
                For Each subIdOrigin In juego.IDsOrigin
                    If idOrigin = subIdOrigin Then
                        idSteam = juego.IDSteam
                        Exit For
                    End If
                Next
            Next

            Return idSteam
        ElseIf devolver = 1 Then
            Dim idOriginFinal As String = String.Empty

            For Each juego In lista
                For Each subIdOrigin In juego.IDsOrigin
                    If idOrigin = subIdOrigin Then
                        idOriginFinal = juego.IDsOrigin(0)
                        Exit For
                    End If
                Next
            Next

            idOriginFinal = idOriginFinal.Replace(":", Nothing)
            Return idOriginFinal
        End If

        Return Nothing

    End Function

    Public Class OriginBBDDEntrada

        Public Titulo As String
        Public IDsOrigin As List(Of String)
        Public IDSteam As String

        Public Sub New(ByVal titulo As String, ByVal idsOrigin As List(Of String), ByVal idSteam As String)
            Me.Titulo = titulo
            Me.IDsOrigin = idsOrigin
            Me.IDSteam = idSteam
        End Sub

    End Class

End Module
