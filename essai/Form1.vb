Public Event System() '.Windows.Forms.DrawItemEventHandler? DrawItem
Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ListView1.View = View.Details

        Dim i As Integer
        ListView1.Columns.Add("Date desinfection", 200, HorizontalAlignment.Left)

        'ListView1.Columns.Add("type desinfection", 100, HorizontalAlignment.Left)
        'ListView1.Columns.Add("tri", 100, HorizontalAlignment.Left)
        Dim position, reste
        Dim filename
        Dim sotr
        Dim png
        Dim trie(My.Computer.FileSystem.GetFiles("C:\H2observer\courbes\").Count - 1)

        For i = 0 To My.Computer.FileSystem.GetFiles("C:\H2observer\courbes\").Count - 1

            filename = (My.Computer.FileSystem.GetFiles("C:\H2observer\courbes\").Item(i))
            sotr = Split(filename, "\")(3)
            '
            png = CDate(sotr.Substring(0, 10))
            '
            trie(i) = png

        Next i
        Array.Sort(trie)
        Array.Reverse(trie)
        '-------------------------------------------------------------------------------------------------------------------------------------
        For i = 0 To My.Computer.FileSystem.GetFiles("C:\H2observer\courbes\").Count - 1
            For j = 0 To My.Computer.FileSystem.GetFiles("C:\H2observer\courbes\").Count - 1

                Dim LVI As New ListViewItem

                filename = (My.Computer.FileSystem.GetFiles("C:\H2observer\courbes\").Item(j))
                sotr = Split(filename, "\")(3)
                png = CDate(sotr.Substring(0, 10))
                If trie(i) = png Then
                    position = sotr.indexof(".")
                    reste = sotr.Substring(0, position)
                    LVI.Text = reste 'trie(i)
                    ' LVI.SubItems.Add(CDate(png))
                    '  LVI.SubItems.Add(png)

                    ListView1.Items.Add(LVI)
                End If


                ' Dim limo = Format(CDate(png), "yyyyMMdd")
                'i.ToString                  'première cellule

                '(i * i).ToString)     'seconde cellule

                'troisième cellule

                'ajout de la ligne
            Next j
        Next i
        '     
        For j = 0 To ListView1.Items.Count - 1

            filename = ListView1.Items(j).ToString '(My.Computer.FileSystem.GetFiles("C:\H2observer\courbes\").Item(j))
            If InStr(1, filename, "_") > 1 Then
                ListView1.Items(j).ForeColor = Color.Red
            End If
        Next j

        ListView1_Click(1, e)
        Label1.Text = ListView1.Items(0).Text

        'ListView1.Items(2).BackColor = Color.Red
        ' ListView1.Items(5).SubItems(1).BackColor = Color.Red
        '      
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim Testou = 0
        Dim Test_date_rech = Format(Today, "yyyyMMdd")
        Dim test_date = Format(Today, "yyyyMMdd")

        Dim foc$    'chemin fichier       lopo = y - 8
        Test_date_rech = "20201127" ' a enlever
        Ext_Ver = "_EB"
        For Testou = 2 To 0 Step -1                                                         ' recherche sur 2 jours desinfections

            'dateR = Date.Now.Date.AddDays(lopo).ToString("yyyyMMdd")

            test_date = Test_date_rech - Testou
            foc$ = "C:\H2observer\data\rclog_" + test_date + Ext_Ver + ".csv"
            If (Not File.Exists(foc$)) Then GoTo saut

            T03.ChartType = SeriesChartType.Spline
            T03.Name = "T03"
            Chart1.Series.Add(T03)
            T03.BorderWidth = 3

            T04.ChartType = SeriesChartType.Spline
            T04.Name = "T04"
            Chart1.Series.Add(T04)
            T04.BorderWidth = 3

            T05.ChartType = SeriesChartType.Spline
            T05.Name = "T05"
            Chart1.Series.Add(T05)
            T05.BorderWidth = 3

            Q03.ChartType = SeriesChartType.Spline
            Q03.Name = "Q03"
            Chart1.Series.Add(Q03)
            Q03.BorderWidth = 3

            Chart1.ChartAreas(0).AxisX.Title = lang(3)              '"Heures de Desinfection"
            Chart1.ChartAreas(0).AxisY.Title = lang(4)                  '"Temperatures"
            Chart1.Palette = ChartColorPalette.Pastel

            '--------------------------enlever
            'foc$ = "C:\essai\rcLog_20230112.csv"
            '----------------------------------------------

            Dim resultat_desf
            Dim date_desf
            Dim table_desf()
            Dim variable_desf = False
            Dim variable_desf1 = False
            Dim desinfection As StreamReader = New StreamReader(foc$)
            Dim ligne_desf As String

            Do
                ligne_desf = desinfection.ReadLine()
                table_desf = Split(ligne_desf, ";")
                If ligne_desf = "" Then Exit Do
                '   If table_desf(6) = "OI + boucle" Or table_desf(6) = "boucle" Or table_desf(6) = "boucle + générateurs" table_desf(7) DESINF CHIMIQUE recirculation

                If table_desf(7) = "recirculation" Or table_desf(7) = "temps d'action" Or table_desf(7) = "rinçage désinfectant" Then table_desf(6) = "chimique" 'pour que la condition or fonctionne
                If table_desf(6) = lang(5) Or table_desf(6) = lang(6) Or table_desf(6) = lang(7) Or table_desf(6) = "chimique" Then

                    'If table_desf(6) = lang(5) Or table_desf(6) = lang(6) Or table_desf(6) = lang(7) Or table_desf(7) = "recirculation" Then
                    variable_desf = True
                    date_desf = test_date
                End If

                resultat_desf = table_desf(6) = lang(5) Or table_desf(6) = lang(6) Or table_desf(6) = lang(7) Or table_desf(6) = "chimique"
                If resultat_desf = False And variable_desf = True Then
                    variable_desf1 = True

                End If

            Loop Until ligne_desf Is Nothing
            desinfection.Close()
            Dim resus = variable_desf = True And variable_desf1 = True
            If resus = False Then GoTo saut


            Dim datte = "" ', heures
            Dim heures = ""
            Dim table()
            Dim cpt = 0     'nbre enregistrements  total    SeriesChartType.Spline                                                           
            ' Dim cpt1 = 0    'nbre enregistrement sup a 80 pour calcul A0
            ' Dim compteur = 0 'nbre enregistrement sup a 80 pour calcul A0T04.IsValueShownAsLabel = True
            ' Dim cpt2 = 0    'nbre enregistrement sup a 80 pour calcul A0

            Dim heur_debut = ""
            Dim heur_fin = ""
            Dim popo = 0
            Dim heur_debut1 = ""
            Dim nom_titre = ""
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim t5 As Double
            Dim t4 As Double
            Dim t3 As Double
            Dim q3 As Double

            Dim cpt2 As Double
            Dim compteur As Double
            Dim pt_debut As Double
            Dim cpt1 As Double
            Dim pt_fin As Double
            Dim pt_fin1 As Double
            pt_fin1 = 0 : pt_fin = 0 : cpt1 = 0 : pt_debut = 0 : compteur = 0 : cpt2 = 0
            t5 = 0 : t3 = 0 : t4 = 0
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



            Dim monStreamReader As StreamReader = New StreamReader(foc$)
            Dim ligne As String
            Do
                ligne = monStreamReader.ReadLine()
                If ligne = "" Then Exit Do
                popo = popo + 1
                table = Split(ligne, ";")

                If table(7) = "recirculation" Or table(7) = "temps d'action" Or table(7) = "rinçage désinfectant" Then table(6) = "chimique" 'pour que la condition or fonctionne
                If table(6) = lang(5) Or table(6) = lang(6) Or table(6) = lang(7) Or table(6) = "chimique" Then
                    If popo = 2 Then
                        test_date = test_date - 1
                        Exit Do
                    End If
                End If
                If popo >= 50 Then Exit Do


            Loop Until ligne Is Nothing
            monStreamReader.Close()


            foc$ = "C:\H2observer\data\rclog_" + test_date + Ext_Ver + ".csv"
            '--------------------------enlever
            'foc$ = "C:\essai\rcLog_20191027.csv"
            '----------------------------------------------
            cpt = 0 : compteur = 0 : cpt1 = 0
            If popo = 2 Then


                '---------------------------------------------------------------------------------------------------- verifier A0 car compteur =0
                Dim monStreamReader1 As StreamReader = New StreamReader(foc$)
                Dim ligne1 As String

                Do
                    ligne1 = monStreamReader1.ReadLine()
                    If ligne1 = "" Then Exit Do

                    table = Split(ligne1, ";")

                    If table(7) = "recirculation" Or table(7) = "temps d'action" Or table(7) = "rinçage désinfectant" Then table(6) = "chimique" 'pour que la condition or fonctionne
                    If table(6) = lang(5) Or table(6) = lang(6) Or table(6) = lang(7) Or table(6) = "chimique" Then

                        datte = table(0)
                        heures = table(1)
                        nom_titre = table(6)

                        t3 = Val(Replace(table(27), ",", "."))
                        t4 = Val(Replace(table(35), ",", "."))
                        t5 = Val(Replace(table(30), ",", "."))

                        If IsNumeric(table(26)) = False Then q3 = 100 Else q3 = Val(Replace(table(26), ",", "."))


                        If table(6) = lang(6) Then                      'BOUCLE
                            T04.Points.AddXY(heures, t4)
                            If t4 >= 80 Then
                                pt_fin = (10 ^ ((t4 - 80) / 10)) * 60 '
                                cpt1 = cpt1 + pt_fin
                            End If
                        End If

                        If table(6) = lang(5) Then                             '"OI + boucle"
                            T03.Points.AddXY(heures, t3)
                            T04.Points.AddXY(heures, t4)
                            If t4 >= 80 Then
                                pt_fin = (10 ^ ((t4 - 80) / 10)) * 60 ''
                                cpt1 = cpt1 + pt_fin
                            End If
                            If t3 >= 80 Then
                                pt_debut = (10 ^ ((t3 - 80) / 10)) * 60 ''
                                compteur = compteur + pt_debut
                            End If

                        End If

                        If table(6) = lang(7) Then                              '"boucle + générateurs"
                            T05.Points.AddXY(heures, t5)
                            T04.Points.AddXY(heures, t4)
                            If t5 >= 80 Then
                                pt_fin1 = (10 ^ ((t5 - 80) / 10)) * 60 '
                                cpt2 = cpt2 + pt_fin1
                            End If
                            If t4 >= 80 Then
                                pt_fin = (10 ^ ((t4 - 80) / 10)) * 60 '
                                cpt1 = cpt1 + pt_fin
                            End If

                        End If

                        If table_desf(6) = "chimique" Then   'DESINF CHIMIQUE"
                            Chart1.ChartAreas(0).AxisY.Title = "Chimique"
                            Q03.Points.AddXY(heures, q3)
                            If q3 = "_x0018__x0018_,_x0018_" Then q3 = 100
                            If q3 >= 70 Then
                                pt_fin = (10 ^ ((q3 - 80) / 10)) * 60 '
                                cpt1 = cpt1 + pt_fin
                            End If

                        End If



                        cpt = cpt + 1
                        If cpt = 1 Then heur_debut = heures

                    End If

                Loop Until ligne1 Is Nothing
                monStreamReader1.Close()

                test_date = test_date + 1
            End If
            '---------------------------------------------------------------------------------------------------------------

            foc$ = "C:\H2observer\data\rclog_" + test_date + Ext_Ver + ".csv"
            '----------------------------------------------------------------------------------------------------
            '--------------------------enlever
            'foc$ = "C:\essai\rcLog_20191027.csv"
            '----------------------------------------------
            Dim monStreamReader2 As StreamReader = New StreamReader(foc$)
            Dim ligne2 As String
            Do
                ligne2 = monStreamReader2.ReadLine()

                If ligne2 = "" Then Exit Do

                table = Split(ligne2, ";")

                If table(7) = "recirculation" Or table(7) = "temps d'action" Or table(7) = "rinçage désinfectant" Then table(6) = "chimique" 'pour que la condition or fonctionne
                If table(6) = lang(5) Or table(6) = lang(6) Or table(6) = lang(7) Or table(6) = "chimique" Then

                    datte = table(0)
                    heures = table(1)
                    nom_titre = table(6)

                    t3 = Val(Replace(table(27), ",", "."))
                    t4 = Val(Replace(table(35), ",", "."))
                    t5 = Val(Replace(table(30), ",", "."))

                    If IsNumeric(table(26)) = False Then q3 = 100 Else q3 = Val(Replace(table(26), ",", "."))
                    ' q3 = Val(Replace(table(26), ",", "."))

                    If table(6) = lang(6) Then                             'boucle
                        T04.Points.AddXY(heures, t4)
                        If t4 >= 80 Then
                            pt_fin = (10 ^ ((t4 - 80) / 10)) * 60 '
                            cpt1 = cpt1 + pt_fin
                        End If
                    End If

                    If table(6) = lang(5) Then                        'oi+boucle
                        T03.Points.AddXY(heures, t3)
                        T04.Points.AddXY(heures, t4)
                        If t4 >= 80 Then
                            pt_fin = (10 ^ ((t4 - 80) / 10)) * 60 '
                            cpt1 = cpt1 + pt_fin
                        End If
                        If t3 >= 80 Then
                            pt_debut = (10 ^ ((t3 - 80) / 10)) * 60 '
                            compteur = compteur + pt_debut
                        End If

                    End If

                    If table(6) = lang(7) Then                       '"boucle + générateurs"
                        T05.Points.AddXY(heures, t5)
                        T04.Points.AddXY(heures, t4)
                        If t5 >= 80 Then
                            pt_fin1 = (10 ^ ((t5 - 80) / 10)) * 60 '
                            cpt2 = cpt2 + pt_fin1
                        End If
                        If t4 >= 80 Then
                            pt_fin = (10 ^ ((t4 - 80) / 10)) * 60 '
                            cpt1 = cpt1 + pt_fin
                        End If

                    End If

                    If table(7) = "recirculation" Or table(7) = "temps d'action" Or table(7) = "rinçage désinfectant" Then   'DESINF CHIMIQUE"
                        Chart1.ChartAreas(0).AxisY.Title = "Chimique"
                        Q03.Points.AddXY(heures, q3)
                        If q3 = "_x0018__x0018_,_x0018_" Then q3 = 100
                        If q3 >= 70 Then
                            pt_fin = (10 ^ ((q3 - 80) / 10)) * 60 '
                            cpt1 = cpt1 + pt_fin
                        End If

                    End If

                    cpt = cpt + 1
                    If cpt = 1 Then heur_debut = heures



                End If

            Loop Until ligne2 Is Nothing
            monStreamReader2.Close()
            If popo = 2 Then heur_debut = heur_debut1
            heur_fin = heures
            Dim h1 = CDate(heur_debut)
            Dim h2 = CDate(heur_fin)
            Dim diff2 As String = (h2 - h1).ToString





            Dim fil1 As String = "C:\H2observer\configuration\config1.cfg"
            Dim i As Integer
            Dim tableau_conf1(24)
            i = 13
            '-----------------------------------------------------------------------
            '------------- chargement config 1 pour récupérer destinataire et centre
            '-----------------------------------------------------------------------
            Dim conf1 As String = "C:\H2observer\configuration\config1.cfg"

            ' ouverture en streamreader pour avoir les accents
            Dim sr3 As StreamReader = New StreamReader(conf1)
            tableau_conf1 = Split(sr3.ReadLine(), ";")
            sr3.Close()

            'FileOpen(1, (fil1), OpenMode.Input)
            'Do
            'tableau_conf1 = Split(LineInput(1), ";")

            'oop Until EOF(1)
            'FileClose(1)

            '--------------------------------------------------------------------- annotation sur la courbe
            Chart1.Annotations.Clear()
            Dim MyAnnotation As New CalloutAnnotation()
            Dim MyAnnotation1 As New CalloutAnnotation()
            Dim MyAnnotation2 As New CalloutAnnotation()
            MyAnnotation.Font = New Font("Arial", 12, FontStyle.Bold)
            MyAnnotation1.Font = New Font("Arial", 12, FontStyle.Bold)
            MyAnnotation2.Font = New Font("Arial", 12, FontStyle.Bold)


            If cpt1 > 10 Then
                MyAnnotation1.AnchorDataPoint = T04.Points(cpt - 3) '(cpt / 7)) '
                MyAnnotation1.Text = lang(8) + Str(Int(cpt1)) '"Annotation sur un Point du graph"255, 255, 128
                If cpt1 >= 12000 Then MyAnnotation1.BackColor = Color.FromArgb(200, 255, 128) Else MyAnnotation1.BackColor = Color.FromArgb(255, 0, 0)
                Chart1.Annotations.Add(MyAnnotation1)
            End If


            If compteur > 10 Then
                MyAnnotation.AnchorDataPoint = T03.Points(3) 'cpt - (cpt / 7))
                MyAnnotation.Text = lang(10) + Str(Int(compteur)) '"Annotation sur un Point du graph"200
                If compteur >= 12000 Then MyAnnotation.BackColor = Color.FromArgb(200, 255, 128) Else MyAnnotation.BackColor = Color.FromArgb(255, 0, 0)
                Chart1.Annotations.Add(MyAnnotation)
            End If

            If cpt2 > 10 Then
                MyAnnotation2.AnchorDataPoint = T05.Points(3) '
                MyAnnotation2.Text = lang(11) + Str(Int(cpt2)) '"Annotation sur un Point du graph"255, 255, 128
                If cpt2 >= 12000 Then MyAnnotation2.BackColor = Color.FromArgb(200, 255, 128) Else MyAnnotation2.BackColor = Color.FromArgb(255, 0, 0)
                Chart1.Annotations.Add(MyAnnotation2)
            End If




            diff2 = diff2.Substring(0, 2) & "h" & diff2.Substring(2 + 1)

            If nom_titre = lang(5) Then nom_titre = lang(13) + " " + datte + " " + lang(14) + " " + diff2 'Else nom_titre = tableau_conf1(0) + " : " + datte + " Desinfection Thermique Boucle Durée " + diff2
            If nom_titre = lang(6) Then nom_titre = lang(13) + " " + datte + " " + lang(16) + " " + diff2
            If nom_titre = lang(7) Then nom_titre = lang(13) + " " + datte + " " + lang(17) + " " + diff2 'Else nom_titre = tableau_conf1(0) + " : " + datte + " Desinfection Thermique Boucle Durée " + diff2


            Chart1.Titles.Clear()
            Dim poloo = tableau_conf1(0)
            Chart1.Titles.Add(poloo)
            Chart1.Titles.Add(nom_titre)
            Chart1.Titles(0).Font = New Font("Arial", 15, FontStyle.Bold)
            Chart1.Titles(1).Font = New Font("Arial", 15, FontStyle.Bold)
            Chart1.Titles.Add(Num_série)
            ' Chart1.Titles.Add((tableau_conf1(0)))
            '
            'verifier chichier deja envoyer  "C:/HERCO/courbe" + Test_date + ".png"
            'format(h1,"HH")
            '----------------------------------------------------------------------------------sauvegarde courbe ---
            '
            '                                                                 penser faire fichier exemple 29-09-2024_OI+BLOUCLE.PNG
            '
            '----------------------------------------------------------------------------------------------------------------------------------
            Dim dot = DateTime.ParseExact(test_date, "yyyyMMdd", Nothing)
            Dim juste = Format(dot, "dd-MM-yyyy")
            Acceuil.courbe_impression = True
            Dim b As New Bitmap(Me.Width, Me.Height)
            Me.DrawToBitmap(b, New Rectangle(0, 0, Me.Width, Me.Height))
            Dim foc1$ = "C:\H2observer\courbes\" + juste + ".png"
            b.Save(foc1$, Imaging.ImageFormat.Png)

            Acceuil.courbe_date = juste

saut:
            Chart1.Series.Clear()
            T03.Points.Clear()
            T04.Points.Clear()
            T05.Points.Clear()
            cpt2 = 0
            cpt1 = 0
            compteur = 0
        Next Testou
        Me.Close()
    End Sub
    End Sub



    Private Sub ListView1_Click(sender As Object, e As EventArgs) Handles ListView1.Click
        Dim jui = "88"
    End Sub

    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs) Handles ListView1.DoubleClick
        Dim po = ListView1.SelectedIndices(0) '.ToString
        Label1.Text = ListView1.Items(po).SubItems(0).Text '& " " & ListView1.Items(po).SubItems(1).Text, "/", "-") 'Dim po = 22
    End Sub
End Class

