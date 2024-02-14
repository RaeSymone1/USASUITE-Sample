using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class PBI69614UpdateScript : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string script = @"--Russell Griffith
update funding 
set EnrolledYear = 2001
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 79
--Sonali Ubhayakar
update funding 
set EnrolledYear = 2002
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 210
--Sara Wiley
update funding 
set EnrolledYear = 2003
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 342
--Theodore Stehney
update funding 
set EnrolledYear = 2003
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 464
--Colin OSullivan
update funding 
set EnrolledYear = 2003
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 467
--Kenneth Wijesinghe
update funding 
set EnrolledYear = 2004
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 816
--John Trifiletti
update funding 
set EnrolledYear = 2005
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1019
--Kristina Cole
update funding 
set EnrolledYear = 2005
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1236
--Amanda Crowell
update funding 
set EnrolledYear = 2006
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1485
--Mike Cook
update funding 
set EnrolledYear = 2006
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1519
--Laura Brown
update funding 
set EnrolledYear = 2007
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1531
--Kyle OMeara
update funding 
set EnrolledYear = 2006
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1577
--Yohann Young - Sumner
update funding 
set EnrolledYear = 2007
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1912
--William Van Besien
update funding 
set EnrolledYear = 2008
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2172
--Erik Beckstrom
update funding 
set EnrolledYear = 2005
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2197
--Celia Paulsen
update funding 
set EnrolledYear = 2009
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2672
--Jill Rowland
update funding 
set EnrolledYear = 2010
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2937
--Michael DAngelo
update funding 
set EnrolledYear = 2010
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2941
--Ryan Stepanek
update funding 
set EnrolledYear = 2010
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2959
--Brittany De Bella
update funding 
set EnrolledYear = 2010
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2988
--Benjamin VanPelt
update funding 
set EnrolledYear = 2010
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3033
--Zachary Otto
update funding 
set EnrolledYear = 2010
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3059
--Bilan Jones
update funding 
set EnrolledYear = 2005
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3177
--Jessica Owzarski
update funding 
set EnrolledYear = 2001
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3293
--Kelley Raines
update funding 
set EnrolledYear = 2011
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3376
--Cindy Martinez
update funding 
set EnrolledYear = 2012
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3542
--Mauranda Elliott
update funding 
set EnrolledYear = 2012
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3560
--Ashley Ragland
update funding 
set EnrolledYear = 2013
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3765
--Kelly Hood
update funding 
set EnrolledYear = 2013
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3792
--Shauna Policicchio
update funding 
set EnrolledYear = 2013
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3917
--Kristina Heinricy
update funding 
set EnrolledYear = 2013
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3926
--Hector Garcia-Lomeli
update funding 
set EnrolledYear = 2013
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4001
--Bryan Conner
update funding 
set EnrolledYear = 2013
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4029
--Ryan Smith
update funding 
set EnrolledYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4135
--Sarah Yoder
update funding 
set EnrolledYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4206
--Stacey Dunson
update funding 
set EnrolledYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4291
--Corey Mize
update funding 
set EnrolledYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4306
--Derek Smith
update funding 
set EnrolledYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4307
--Caroline Holcomb
update funding 
set EnrolledYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4309
--Michael Thompson
update funding 
set EnrolledYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4319
--Allison Holt
update funding 
set EnrolledYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4323
--Nicholas Schuchhardt
update funding 
set EnrolledYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4367
--Marcellus Williams
update funding 
set EnrolledYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4391
--Kathryn Johnston
update funding 
set EnrolledYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4423
--Whitney Merrill
update funding 
set EnrolledYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4424
--Bridget Pelletier-Ross
update funding 
set EnrolledYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4484
--Kayla Mormak
update funding 
set EnrolledYear = 2014
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4494
--Miranda Hamilton
update funding 
set EnrolledYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4521
--William Morgan
update funding 
set EnrolledYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4573
--Shane Rogers
update funding 
set EnrolledYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4594
--Rahmira Rufus
update funding 
set EnrolledYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4603
--Miriam Feldhausen
update funding 
set EnrolledYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5668
--Cervando Banuelos
update funding 
set EnrolledYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5671
--Joseph Wilson
update funding 
set EnrolledYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5773
--Emily Leathers
update funding 
set EnrolledYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5785
--Shelby Anderson
update funding 
set EnrolledYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5841
--Sara Mitchell
update funding 
set EnrolledYear = 2015
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5871
--Jesus Carrete-Lopez
update funding 
set EnrolledYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5961
--Ricardo Castilla
update funding 
set EnrolledYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5984
--Mario J Vazquez
update funding 
set EnrolledYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 6099
--Abrielle Holloway
update funding 
set EnrolledYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 6102
--David Afield
update funding 
set EnrolledYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 6144
--Dalton Brucker-Hahn
update funding 
set EnrolledYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 6166
--Shane Bennett
update funding 
set EnrolledYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7244
--Adrian Hy
update funding 
set EnrolledYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7246
--Melissa Hannis
update funding 
set EnrolledYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7289
--Travis Wright
update funding 
set EnrolledYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7301
--Ronak Patel
update funding 
set EnrolledYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7316
--Megan Isaacs
update funding 
set EnrolledYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7457
--Marcus Brown
update funding 
set EnrolledYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7501
--LeTasha McFarlane-Garrard
update funding 
set EnrolledYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7663
--Terrence Kvitchko
update funding 
set EnrolledYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7711
--Michael Garippo
update funding 
set EnrolledYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7757
--Emily OHanlon
update funding 
set EnrolledYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7794
--Mbi Mbah Anchi
update funding 
set EnrolledYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7797
--Ming Chen
update funding 
set EnrolledYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7801
--JoHannah Couture
update funding 
set EnrolledYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7859
--Lauren Good
update funding 
set EnrolledYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7968
--MaryAnn VanValkenburg
update funding 
set EnrolledYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 8026
--Kristen Aiken
update funding 
set EnrolledYear = 2017
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 8030
--George Harter
update funding 
set EnrolledYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 8064
--Michael Walker
update funding 
set EnrolledYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 8065
--James Bell
update funding 
set EnrolledYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9428
--Colby Parker
update funding 
set EnrolledYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9431
--Beckett Browning
update funding 
set EnrolledYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9537
--Lydia Speirs
update funding 
set EnrolledYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9545
--Jennifer Guerra
update funding 
set EnrolledYear = 2018
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9564
--Nuria Pacheco
update funding 
set EnrolledYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10788
--Travis Wright
update funding 
set EnrolledYear = 2016
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10799
--Ash Harper
update funding 
set EnrolledYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10872
--Sophia Gatsios
update funding 
set EnrolledYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10927
--Mackenzie Oestreich
update funding 
set EnrolledYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10928
--Courtney Grubbs
update funding 
set EnrolledYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10941
--Yusef Yassin
update funding 
set EnrolledYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11022
--Felix Baez Merced
update funding 
set EnrolledYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11023
--Thomas McCullough
update funding 
set EnrolledYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11025
--Janine Willms Brasseaux
update funding 
set EnrolledYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11077
--Anna Opsahl
update funding 
set EnrolledYear = 2019
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11124
--Joseph Corona Vazquez
update funding 
set EnrolledYear = 2020
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11572
--Skylar Rumpca
update funding 
set EnrolledYear = 2020
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11585
--Carly Winters
update funding 
set EnrolledYear = 2020
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11752
--Paul Duhe
update funding 
set EnrolledYear = 2021
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11828
--Henry Motta
update funding 
set EnrolledYear = 2021
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11960
--Samuel Dekemper
update funding 
set EnrolledYear = 2022
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13684
--Dino Bonaldo
update funding 
set EnrolledYear = 2022
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13739
--Eduard Ramos
update funding 
set EnrolledYear = 2022
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13755
--Daniel Daszkiewicz
update funding 
set EnrolledYear = 2022
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13768
--Vincent Pisani
update funding 
set EnrolledYear = 2023
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13871
--Sankofa Benzo
update funding 
set EnrolledYear = 2023
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13884
--Jessica Berrios
update funding 
set EnrolledYear = 2023
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13885
--William Fritts
update funding 
set EnrolledYear = 2023
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13886
--Zaq Humphrey
update funding 
set EnrolledYear = 2022
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13897
";
            migrationBuilder.Sql(script);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
