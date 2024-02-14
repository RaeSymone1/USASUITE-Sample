using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class PBI69613UpdateScript : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string script = @"--Russell Griffith
update funding 
set FundingEndYear = 2002
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 79
--Ryan Leigland
update funding 
set FundingEndYear = 2004
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 96
--Samantha Fields
update funding 
set FundingEndYear = 2004
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 128
--Sonali Ubhayakar
update funding 
set FundingEndYear = 2003
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 210
--Sara Wiley
update funding 
set FundingEndYear = 2004
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 342
--Theodore Stehney
update funding 
set FundingEndYear = 2005
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 464
--Colin OSullivan
update funding 
set FundingEndYear = 2004
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 467
--Kenneth Wijesinghe
update funding 
set FundingEndYear = 2006
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 816
--John Trifiletti
update funding 
set FundingEndYear = 2007
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1019
--James Doyle
update funding 
set FundingEndYear = 2006
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1022
--Kristina Cole
update funding 
set FundingEndYear = 2007
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1236
--Chamel Evans
update funding 
set FundingEndYear = 2008
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1475
--Mike Cook
update funding 
set FundingEndYear = 2008
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1519
--Laura Brown
update funding 
set FundingEndYear = 2008
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1531
--Kyle OMeara
update funding 
set FundingEndYear = 2008
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1577
--Samuel Clements
update funding 
set FundingEndYear = 2008
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1582
--Emily Ecoff
update funding 
set FundingEndYear = 2009
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1872
--Chad Johnson
update funding 
set FundingEndYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1894
--Yohann Young - Sumner
update funding 
set FundingEndYear = 2009
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1912
--Matthew Lempka
update funding 
set FundingEndYear = 2010
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2156
--William Van Besien
update funding 
set FundingEndYear = 2010
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2172
--Erik Beckstrom
update funding 
set FundingEndYear = 2007
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2197
--Jonathan Leigh
update funding 
set FundingEndYear = 2011
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2563
--John Doyle
update funding 
set FundingEndYear = 2011
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2576
--Adrien Cheval
update funding 
set FundingEndYear = 2010
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2584
--Celia Paulsen
update funding 
set FundingEndYear = 2011
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2672
--Jill Rowland
update funding 
set FundingEndYear = 2013
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2937
--Michael DAngelo
update funding 
set FundingEndYear = 2012
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2941
--Ryan Stepanek
update funding 
set FundingEndYear = 2012
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2959
--Matthew Fillette
update funding 
set FundingEndYear = 2010
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2960
--Brittany De Bella
update funding 
set FundingEndYear = 2012
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2988
--Benjamin VanPelt
update funding 
set FundingEndYear = 2012
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3033
--Zachary Otto
update funding 
set FundingEndYear = 2012
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3059
--Bilan Jones
update funding 
set FundingEndYear = 2007
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3177
--Jessica Owzarski
update funding 
set FundingEndYear = 2012
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3293
--Kelley Raines
update funding 
set FundingEndYear = 2013
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3376
--BENJAMIN NKRUMAH
update funding 
set FundingEndYear = 2013
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3499
--Cindy Martinez
update funding 
set FundingEndYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3542
--Hans Vargas Silva
update funding 
set FundingEndYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3558
--Mauranda Elliott
update funding 
set FundingEndYear = 2013
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3560
--Thomas McCarthy
update funding 
set FundingEndYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3680
--Alex Doyal
update funding 
set FundingEndYear = 2013
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3755
--Ashley Ragland
update funding 
set FundingEndYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3765
--Kelly Hood
update funding 
set FundingEndYear = 2013
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3792
--Benjamin Schmidt
update funding 
set FundingEndYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3877
--Shauna Policicchio
update funding 
set FundingEndYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3917
--Kristina Heinricy
update funding 
set FundingEndYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3926
--Hector Garcia-Lomeli
update funding 
set FundingEndYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4001
--Bryan Conner
update funding 
set FundingEndYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4029
--Michael Cook
update funding 
set FundingEndYear = 2008
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4063
--Ryan Smith
update funding 
set FundingEndYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4135
--Sarah Yoder
update funding 
set FundingEndYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4206
--Arthur Jicha
update funding 
set FundingEndYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4219
--Stacey Dunson
update funding 
set FundingEndYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4291
--Corey Mize
update funding 
set FundingEndYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4306
--Derek Smith
update funding 
set FundingEndYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4307
--Caroline Holcomb
update funding 
set FundingEndYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4309
--Michael Thompson
update funding 
set FundingEndYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4319
--Allison Holt
update funding 
set FundingEndYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4323
--Nicholas Schuchhardt
update funding 
set FundingEndYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4367
--James Williams
update funding 
set FundingEndYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4385
--Marcellus Williams
update funding 
set FundingEndYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4391
--Kathryn Johnston
update funding 
set FundingEndYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4423
--Whitney Merrill
update funding 
set FundingEndYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4424
--Bridget Pelletier-Ross
update funding 
set FundingEndYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4484
--Kayla Mormak
update funding 
set FundingEndYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4494
--Miranda Hamilton
update funding 
set FundingEndYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4521
--William Morgan
update funding 
set FundingEndYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4573
--Shane Rogers
update funding 
set FundingEndYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4594
--Rahmira Rufus
update funding 
set FundingEndYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4603
--Miriam Feldhausen
update funding 
set FundingEndYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5668
--Cervando Banuelos
update funding 
set FundingEndYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5671
--Joseph Wilson
update funding 
set FundingEndYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5773
--Nicholas Kantor
update funding 
set FundingEndYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5778
--Emily Leathers
update funding 
set FundingEndYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5785
--Shelby Anderson
update funding 
set FundingEndYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5841
--Sara Mitchell
update funding 
set FundingEndYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5871
--Jesus Carrete-Lopez
update funding 
set FundingEndYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5961
--Ricardo Castilla
update funding 
set FundingEndYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5984
--Mario J Vazquez
update funding 
set FundingEndYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 6099
--Abrielle Holloway
update funding 
set FundingEndYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 6102
--David Afield
update funding 
set FundingEndYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 6144
--Dalton Brucker-Hahn
update funding 
set FundingEndYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 6166
--Shane Bennett
update funding 
set FundingEndYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7244
--Adrian Hy
update funding 
set FundingEndYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7246
--Joanna Hall
update funding 
set FundingEndYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7275
--Melissa Hannis
update funding 
set FundingEndYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7289
--Ronak Patel
update funding 
set FundingEndYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7316
--Megan Isaacs
update funding 
set FundingEndYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7457
--Marcus Brown
update funding 
set FundingEndYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7501
--Amber Paris
update funding 
set FundingEndYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7561
--LeTasha McFarlane-Garrard
update funding 
set FundingEndYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7663
--Terrence Kvitchko
update funding 
set FundingEndYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7711
--Michael Garippo
update funding 
set FundingEndYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7757
--Emily OHanlon
update funding 
set FundingEndYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7794
--Mbi Mbah Anchi
update funding 
set FundingEndYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7797
--Ming Chen
update funding 
set FundingEndYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7801
--Rachael Dunn
update funding 
set FundingEndYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7855
--JoHannah Couture
update funding 
set FundingEndYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7859
--Tempestt Neal
update funding 
set FundingEndYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7861
--Lauren Good
update funding 
set FundingEndYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7968
--Evan White
update funding 
set FundingEndYear = 2020
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7982
--MaryAnn VanValkenburg
update funding 
set FundingEndYear = 2020
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 8026
--Kristen Aiken
update funding 
set FundingEndYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 8030
--George Harter
update funding 
set FundingEndYear = 2020
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 8064
--Michael Walker
update funding 
set FundingEndYear = 2020
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 8065
--James Bell
update funding 
set FundingEndYear = 2020
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9428
--Colby Parker
update funding 
set FundingEndYear = 2021
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9431
--Mattisen Halliday
update funding 
set FundingEndYear = 2020
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9481
--Hannah Kleinheider
update funding 
set FundingEndYear = 2020
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9512
--Beckett Browning
update funding 
set FundingEndYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9537
--Lydia Speirs
update funding 
set FundingEndYear = 2020
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9545
--William Johnson
update funding 
set FundingEndYear = 2021
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9551
--Jennifer Guerra
update funding 
set FundingEndYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9564
--Nuria Pacheco
update funding 
set FundingEndYear = 2021
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10788
--Travis Wright
update funding 
set FundingEndYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10799
--Ash Harper
update funding 
set FundingEndYear = 2020
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10872
--Blair Doyle
update funding 
set FundingEndYear = 2020
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10894
--Sophia Gatsios
update funding 
set FundingEndYear = 2020
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10927
--Mackenzie Oestreich
update funding 
set FundingEndYear = 2022
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10928
--Courtney Grubbs
update funding 
set FundingEndYear = 2022
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10941
--Yusef Yassin
update funding 
set FundingEndYear = 2020
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11022
--Felix Baez Merced
update funding 
set FundingEndYear = 2020
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11023
--Thomas McCullough
update funding 
set FundingEndYear = 2021
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11025
--Janine Willms Brasseaux
update funding 
set FundingEndYear = 2021
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11077
--Anna Opsahl
update funding 
set FundingEndYear = 2022
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11124
--Benjamin Hughes
update funding 
set FundingEndYear = 2021
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11277
--Alexandra Mattika
update funding 
set FundingEndYear = 2021
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11282
--Javier Franco
update funding 
set FundingEndYear = 2022
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11352
--Oscar Bautista Chia
update funding 
set FundingEndYear = 2022
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11483
--Joseph Corona Vazquez
update funding 
set FundingEndYear = 2022
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11572
--Skylar Rumpca
update funding 
set FundingEndYear = 2021
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11585
--Carly Winters
update funding 
set FundingEndYear = 2022
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11752
--Paul Duhe
update funding 
set FundingEndYear = 2023
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11828
--Henry Motta
update funding 
set FundingEndYear = 2022
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11960
--Bradley Schlauder
update funding 
set FundingEndYear = 2024
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 12057
--Leojaris Brujan
update funding 
set FundingEndYear = 2023
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 12114
--John Melton
update funding 
set FundingEndYear = 2023
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13148
--Evan White
update funding 
set FundingEndYear = 2023
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13223
--Valrie Jules
update funding 
set FundingEndYear = 2022
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13389
--Matthew Jones
update funding 
set FundingEndYear = 2024
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13474
--Samuel Dekemper
update funding 
set FundingEndYear = 2024
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13684
--Andrew Turner
update funding 
set FundingEndYear = 2024
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13706
--Dino Bonaldo
update funding 
set FundingEndYear = 2023
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13739
--Charlotte Negron Negron Lopez De Vic
update funding 
set FundingEndYear = 2023
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13751
--Eduard Ramos
update funding 
set FundingEndYear = 2024
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13755
--Danay Rumbaut
update funding 
set FundingEndYear = 2025
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13765
--Daniel Daszkiewicz
update funding 
set FundingEndYear = 2025
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13768
--Allan Gonzalez Benjume
update funding 
set FundingEndYear = 2025
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13806
--Diamaris Gonz�lez-Rodr�guez
update funding 
set FundingEndYear = 2025
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13837
--Vincent Pisani
update funding 
set FundingEndYear = 2024
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13871
--Sankofa Benzo
update funding 
set FundingEndYear = 2025
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13884
--Jessica Berrios
update funding 
set FundingEndYear = 2025
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13885
--William Fritts
update funding 
set FundingEndYear = 2024
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13886
--Zaq Humphrey
update funding 
set FundingEndYear = 2025
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13897";
            migrationBuilder.Sql(script);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
