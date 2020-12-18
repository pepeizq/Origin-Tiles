Module OriginBBDD

    'https://api3.origin.com/supercat/GB/en_GB/supercat-PCWIN_MAC-GB-en_GB.json.gz

    Public Function BuscarenListado(idOrigin As String, devolver As Integer)

        Dim lista As New List(Of OriginBBDDEntrada) From {
            New OriginBBDDEntrada("A Way Out", New List(Of String) From {"Origin.OFR.50.0001900", "Origin.OFR.50.0002487"}, "1222700"),
            New OriginBBDDEntrada("Apex Legends", New List(Of String) From {"Origin.OFR.50.0002694"}, "1172470"),
            New OriginBBDDEntrada("Anthem", New List(Of String) From {"Origin.OFR.50.0001521", "Origin.OFR.50.0002755", "Origin.OFR.50.0002760", "Origin.OFR.50.0002980"}, Nothing),
            New OriginBBDDEntrada("Battlefield 1", New List(Of String) From {"Origin.OFR.50.0000557", "Origin.OFR.50.0001380", "Origin.OFR.50.0001381", "Origin.OFR.50.0002321", "Origin.OFR.50.0002323", "Origin.OFR.50.0002324"}, "1238840"),
            New OriginBBDDEntrada("Battlefield 1942", New List(Of String) From {"OFB-EAST:56186"}, Nothing),
            New OriginBBDDEntrada("Battlefield 2 Complete Collection", New List(Of String) From {"DR:78869400"}, Nothing),
            New OriginBBDDEntrada("Battlefield 3", New List(Of String) From {"DR:225064100", "DR:224766400"}, "1238820"),
            New OriginBBDDEntrada("Battlefield 4 Premium Edition", New List(Of String) From {"OFB-EAST:109552316"}, "1238860"),
            New OriginBBDDEntrada("Battlefield Bad Company 2", New List(Of String) From {"DR:156691300"}, "24960"),
            New OriginBBDDEntrada("Battlefield Hardline Ultimate Edition", New List(Of String) From {"Origin.OFR.50.0000846"}, "1238880"),
            New OriginBBDDEntrada("Battlefield V Definitive Edition", New List(Of String) From {"Origin.OFR.50.0002942"}, "1238810"),
            New OriginBBDDEntrada("Bejeweled 3", New List(Of String) From {"OFB-EAST:49753"}, "78000"),
            New OriginBBDDEntrada("Burnout Paradise Remastered", New List(Of String) From {"Origin.OFR.50.0002535"}, "1238080"),
            New OriginBBDDEntrada("Burnout Paradise The Ultimate Box", New List(Of String) From {"DR:106479900"}, "24740"),
            New OriginBBDDEntrada("Command & Conquer", New List(Of String) From {"OFB-EAST:52016"}, Nothing),
            New OriginBBDDEntrada("Command & Conquer 3: Tiberium Wars", New List(Of String) From {"OFB-EAST:52210"}, "24790"),
            New OriginBBDDEntrada("Command & Conquer 4: Tiberian Twilight", New List(Of String) From {"OFB-EAST:52212"}, "47700"),
            New OriginBBDDEntrada("Command & Conquer Generals", New List(Of String) From {"OFB-EAST:52209"}, Nothing),
            New OriginBBDDEntrada("Command & Conquer Red Alert", New List(Of String) From {"OFB-EAST:52017"}, Nothing),
            New OriginBBDDEntrada("Command & Conquer Red Alert 2", New List(Of String) From {"OFB-EAST:52019"}, Nothing),
            New OriginBBDDEntrada("Command & Conquer Red Alert 3", New List(Of String) From {"OFB-EAST:52211"}, "17480"),
            New OriginBBDDEntrada("Command & Conquer Red Alert 3 Uprising", New List(Of String) From {"DR:111298000"}, "24800"),
            New OriginBBDDEntrada("Command & Conquer Remastered Collection", New List(Of String) From {"Origin.OFR.50.0004022"}, "1213210"),
            New OriginBBDDEntrada("Command & Conquer Renegade", New List(Of String) From {"OFB-EAST:52208"}, Nothing),
            New OriginBBDDEntrada("Command & Conquer Tiberian Sun", New List(Of String) From {"OFB-EAST:52018"}, Nothing),
            New OriginBBDDEntrada("Crysis", New List(Of String) From {"DR:77033800"}, "17300"),
            New OriginBBDDEntrada("Crysis 2", New List(Of String) From {"OFB-EAST:49464"}, "108800"),
            New OriginBBDDEntrada("Crysis 3", New List(Of String) From {"OFB-EAST:49562"}, "1282690"),
            New OriginBBDDEntrada("Crysis Warhead", New List(Of String) From {"DR:105905500"}, "17330"),
            New OriginBBDDEntrada("Dead Space", New List(Of String) From {"OFB-EAST:109547518"}, "17470"),
            New OriginBBDDEntrada("Dead Space 2", New List(Of String) From {"OFB-EAST:55619"}, "47780"),
            New OriginBBDDEntrada("Dead Space 3", New List(Of String) From {"OFB-EAST:50885"}, "1238060"),
            New OriginBBDDEntrada("Dragon Age II", New List(Of String) From {"DR:201797000"}, "1238040"),
            New OriginBBDDEntrada("Dragon Age Inquisition", New List(Of String) From {"Origin.OFR.50.0001131"}, "1222690"),
            New OriginBBDDEntrada("Dragon Age Origins Ultimate Edition", New List(Of String) From {"DR:208591800"}, "47810"),
            New OriginBBDDEntrada("Dungeon Keeper", New List(Of String) From {"DR:231813000"}, Nothing),
            New OriginBBDDEntrada("Dungeon Keeper 2", New List(Of String) From {"OFB-EAST:39131"}, Nothing),
            New OriginBBDDEntrada("Fe", New List(Of String) From {"Origin.OFR.50.0001944"}, "1225580"),
            New OriginBBDDEntrada("FIFA 20 Ultimate Edition", New List(Of String) From {"Origin.OFR.50.0003453"}, Nothing),
            New OriginBBDDEntrada("FIFA 21 Ultimate Edition", New List(Of String) From {"Origin.OFR.50.0004195"}, "1313860"),
            New OriginBBDDEntrada("Jade Empire: Special Edition", New List(Of String) From {"OFB-EAST:1000017"}, "7110"),
            New OriginBBDDEntrada("Madden NFL 20 Ultimate Edition", New List(Of String) From {"Origin.OFR.50.0003417"}, Nothing),
            New OriginBBDDEntrada("Madden NFL 21 Ultimate Edition", New List(Of String) From {"Origin.OFR.50.0004125"}, "1239520"),
            New OriginBBDDEntrada("Mass Effect", New List(Of String) From {"DR:102427200"}, "17460"),
            New OriginBBDDEntrada("Mass Effect 2", New List(Of String) From {"OFB-EAST:56694"}, "24980"),
            New OriginBBDDEntrada("Mass Effect 3", New List(Of String) From {"DR:229644400"}, "1238020"),
            New OriginBBDDEntrada("Mass Effect Andromeda", New List(Of String) From {"Origin.OFR.50.0001539"}, "1238000"),
            New OriginBBDDEntrada("Medal of Honor", New List(Of String) From {"DR:215619100"}, "24840"),
            New OriginBBDDEntrada("Medal of Honor: Airbone", New List(Of String) From {"DR:80586700"}, "47790"),
            New OriginBBDDEntrada("Medal of Honor: Allied Assault", New List(Of String) From {"OFB-EAST:1000011"}, Nothing),
            New OriginBBDDEntrada("Medal of Honor: Pacific Assault", New List(Of String) From {"Origin.OFR.50.0000357"}, Nothing),
            New OriginBBDDEntrada("Mirror's Edge", New List(Of String) From {"DR:106999100"}, "17410"),
            New OriginBBDDEntrada("Mirror's Edge Catalyst", New List(Of String) From {"Origin.OFR.50.0001000"}, "1233570"),
            New OriginBBDDEntrada("Need For Speed Deluxe Edition", New List(Of String) From {"Origin.OFR.50.0001009"}, "1262540"),
            New OriginBBDDEntrada("Need For Speed Heat Deluxe Edition", New List(Of String) From {"Origin.OFR.50.0003514"}, "1222680"),
            New OriginBBDDEntrada("Need For Speed Hot Pursuit", New List(Of String) From {"DR:210703600"}, Nothing),
            New OriginBBDDEntrada("Need For Speed Hot Pursuit Remastered", New List(Of String) From {"Origin.OFR.50.0004030"}, "1328660"),
            New OriginBBDDEntrada("Need For Speed Payback Deluxe Edition", New List(Of String) From {"Origin.OFR.50.0002167"}, "1262580"),
            New OriginBBDDEntrada("Need For Speed Rivals Complete Edition", New List(Of String) From {"Origin.OFR.50.0000676"}, "1262600"),
            New OriginBBDDEntrada("Peggle Deluxe", New List(Of String) From {"Origin.OFR.50.0000461"}, "3480"),
            New OriginBBDDEntrada("Plants vs. Zombies: Battle for Neighborville", New List(Of String) From {"Origin.OFR.50.0002623", "Origin.OFR.50.0003634", "Origin.OFR.50.0003675", "Origin.OFR.50.0004019"}, "1262240"),
            New OriginBBDDEntrada("Plants vs. Zombies: Game of the Year", New List(Of String) From {"OFB-EAST:48217"}, "3590"),
            New OriginBBDDEntrada("Plants vs. Zombies: Garden Warfare 2", New List(Of String) From {"Origin.OFR.50.0000786", "Origin.OFR.50.0001051"}, Nothing),
            New OriginBBDDEntrada("Rocket Arena", New List(Of String) From {"Origin.OFR.50.0003997"}, "1233550"),
            New OriginBBDDEntrada("Sea of Solitude", New List(Of String) From {"Origin.OFR.50.0002405"}, "1225590"),
            New OriginBBDDEntrada("Shank", New List(Of String) From {"DR:224434500"}, "6120"),
            New OriginBBDDEntrada("SimCity 2000 Special Edition", New List(Of String) From {"DR:235664600"}, Nothing),
            New OriginBBDDEntrada("SimCity 4 Deluxe Edition", New List(Of String) From {"DR:198095500"}, "24780"),
            New OriginBBDDEntrada("SimCity", New List(Of String) From {"OFB-EAST:48205", "Origin.OFR.50.0000029", "Origin.OFR.50.0000741"}, Nothing),
            New OriginBBDDEntrada("Spore", New List(Of String) From {"DR:91619200"}, "17390"),
            New OriginBBDDEntrada("Star Wars Battlefront", New List(Of String) From {"Origin.OFR.50.0001211"}, "1237980"),
            New OriginBBDDEntrada("Star Wars Battlefront II", New List(Of String) From {"Origin.OFR.50.0001523", "Origin.OFR.50.0002015", "Origin.OFR.50.0002898"}, "1237950"),
            New OriginBBDDEntrada("Star Wars Jedi: Fallen Order", New List(Of String) From {"Origin.OFR.50.0003794", "Origin.OFR.50.0003797", "Origin.OFR.50.0003815"}, "1172380"),
            New OriginBBDDEntrada("Star Wars Squadrons", New List(Of String) From {"Origin.OFR.50.0003112", "Origin.OFR.50.0003896", "Origin.OFR.50.0004099"}, "1222730"),
            New OriginBBDDEntrada("Star Wars The Old Republic", New List(Of String) From {"DR:231255400"}, "1286830"),
            New OriginBBDDEntrada("The Saboteur", New List(Of String) From {"DR:106999600"}, Nothing),
            New OriginBBDDEntrada("The Sims 3", New List(Of String) From {"OFB-EAST:55107"}, "47890"),
            New OriginBBDDEntrada("The Sims 4", New List(Of String) From {"OFB-EAST:109552299", "OFB-EAST:109552408", "OFB-EAST:109552410", "OFB-EAST:109552414"}, "1222670"),
            New OriginBBDDEntrada("Theme Hospital", New List(Of String) From {"Origin.OFR.50.0000500"}, Nothing),
            New OriginBBDDEntrada("Titanfall Deluxe Edition", New List(Of String) From {"Origin.OFR.50.0000739"}, "1454890"),
            New OriginBBDDEntrada("Titanfall 2", New List(Of String) From {"Origin.OFR.50.0001451", "Origin.OFR.50.0001452", "Origin.OFR.50.0001455", "Origin.OFR.50.0001456"}, "1237970"),
            New OriginBBDDEntrada("Unravel", New List(Of String) From {"Origin.OFR.50.0000823"}, "1225560"),
            New OriginBBDDEntrada("Unravel Two", New List(Of String) From {"Origin.OFR.50.0002403", "Origin.OFR.50.0002522"}, "1225570")
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
