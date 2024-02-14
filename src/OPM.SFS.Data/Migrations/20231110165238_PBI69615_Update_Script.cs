using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class PBI69615UpdateScript : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string script = @"--Russell Griffith
update funding 
set TotalAcademicTerms = 2
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 79
--Sonali Ubhayakar
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 210
--Sara Wiley
update funding 
set TotalAcademicTerms = 2
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 342
--Theodore Stehney
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 464
--Colin OSullivan
update funding 
set TotalAcademicTerms = 2
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 467
--Kenneth Wijesinghe
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 816
--John Trifiletti
update funding 
set TotalAcademicTerms = 5
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1019
--Kristina Cole
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1236
--Amanda Crowell
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1485
--Mike Cook
update funding 
set TotalAcademicTerms = 5
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1519
--Laura Brown
update funding 
set TotalAcademicTerms = 3
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1531
--Kyle OMeara
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1577
--Yohann Young - Sumner
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1912
--William Van Besien
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2172
--Erik Beckstrom
update funding 
set TotalAcademicTerms = 5
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2197
--Celia Paulsen
update funding 
set TotalAcademicTerms = 5
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2672
--Jill Rowland
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2937
--Michael DAngelo
update funding 
set TotalAcademicTerms = 5
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2941
--Ryan Stepanek
update funding 
set TotalAcademicTerms = 5
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2959
--Brittany De Bella
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2988
--Benjamin VanPelt
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3033
--Zachary Otto
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3059
--Bilan Jones
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3177
--Jessica Owzarski
update funding 
set TotalAcademicTerms = 2
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3293
--Kelley Raines
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3376
--Cindy Martinez
update funding 
set TotalAcademicTerms = 5
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3542
--Mauranda Elliott
update funding 
set TotalAcademicTerms = 2
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3560
--Ashley Ragland
update funding 
set TotalAcademicTerms = 3
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3765
--Kelly Hood
update funding 
set TotalAcademicTerms = 1
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3792
--Shauna Policicchio
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3917
--Kristina Heinricy
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3926
--Hector Garcia-Lomeli
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4001
--Bryan Conner
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4029
--Ryan Smith
update funding 
set TotalAcademicTerms = 1
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4135
--Sarah Yoder
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4206
--Christopher Hatton
update funding 
set TotalAcademicTerms = 0
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4254
--Stacey Dunson
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4291
--Corey Mize
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4306
--Derek Smith
update funding 
set TotalAcademicTerms = 2
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4307
--Caroline Holcomb
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4309
--Michael Thompson
update funding 
set TotalAcademicTerms = 2
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4319
--Allison Holt
update funding 
set TotalAcademicTerms = 5
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4323
--Nicholas Schuchhardt
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4367
--Marcellus Williams
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4391
--Kathryn Johnston
update funding 
set TotalAcademicTerms = 5
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4423
--Whitney Merrill
update funding 
set TotalAcademicTerms = 2
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4424
--Bridget Pelletier-Ross
update funding 
set TotalAcademicTerms = 3
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4484
--Kayla Mormak
update funding 
set TotalAcademicTerms = 2
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4494
--Miranda Hamilton
update funding 
set TotalAcademicTerms = 5
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4521
--William Morgan
update funding 
set TotalAcademicTerms = 5
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4573
--Shane Rogers
update funding 
set TotalAcademicTerms = 1
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4594
--Rahmira Rufus
update funding 
set TotalAcademicTerms = 5
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4603
--Miriam Feldhausen
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5668
--Cervando Banuelos
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5671
--Joseph Wilson
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5773
--Emily Leathers
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5785
--Shelby Anderson
update funding 
set TotalAcademicTerms = 2
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5841
--Sara Mitchell
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5871
--Jesus Carrete-Lopez
update funding 
set TotalAcademicTerms = 3
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5961
--Ricardo Castilla
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5984
--Mario J Vazquez
update funding 
set TotalAcademicTerms = 7
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 6099
--Abrielle Holloway
update funding 
set TotalAcademicTerms = 2
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 6102
--David Afield
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 6144
--Dalton Brucker-Hahn
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 6166
--Shane Bennett
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7244
--Adrian Hy
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7246
--Melissa Hannis
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7289
--Ronak Patel
update funding 
set TotalAcademicTerms = 2
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7316
--Megan Isaacs
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7457
--Marcus Brown
update funding 
set TotalAcademicTerms = 2
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7501
--LeTasha McFarlane-Garrard
update funding 
set TotalAcademicTerms = 3
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7663
--Terrence Kvitchko
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7711
--Michael Garippo
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7757
--Emily OHanlon
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7794
--Mbi Mbah Anchi
update funding 
set TotalAcademicTerms = 5
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7797
--Ming Chen
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7801
--JoHannah Couture
update funding 
set TotalAcademicTerms = 2
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7859
--Lauren Good
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7968
--MaryAnn VanValkenburg
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 8026
--Kristen Aiken
update funding 
set TotalAcademicTerms = 5
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 8030
--George Harter
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 8064
--Michael Walker
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 8065
--James Bell
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9428
--Colby Parker
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9431
--Beckett Browning
update funding 
set TotalAcademicTerms = 2
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9537
--Lydia Speirs
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9545
--Jennifer Guerra
update funding 
set TotalAcademicTerms = 2
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9564
--Nuria Pacheco
update funding 
set TotalAcademicTerms = 7
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10788
--Travis Wright
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10799
--Ash Harper
update funding 
set TotalAcademicTerms = 3
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10872
--Sophia Gatsios
update funding 
set TotalAcademicTerms = 2
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10927
--Mackenzie Oestreich
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10928
--Courtney Grubbs
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10941
--Yusef Yassin
update funding 
set TotalAcademicTerms = 3
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11022
--Felix Baez Merced
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11023
--Thomas McCullough
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11025
--Janine Willms Brasseaux
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11077
--Anna Opsahl
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11124
--Joseph Corona Vazquez
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11572
--Skylar Rumpca
update funding 
set TotalAcademicTerms = 2
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11585
--Carly Winters
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11752
--Paul Duhe
update funding 
set TotalAcademicTerms = 8
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11828
--Henry Motta
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11960
--Samuel Dekemper
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13684
--Dino Bonaldo
update funding 
set TotalAcademicTerms = 3
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13739
--Eduard Ramos
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13755
--Fabian Garcia Romero
update funding 
set TotalAcademicTerms = 9
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13758
--Daniel Daszkiewicz
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13768
--Will Melendez
update funding 
set TotalAcademicTerms = 7
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13835
--Vincent Pisani
update funding 
set TotalAcademicTerms = 3
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13871
--Sankofa Benzo
update funding 
set TotalAcademicTerms = 6
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13884
--Jessica Berrios
update funding 
set TotalAcademicTerms = 5
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13885
--William Fritts
update funding 
set TotalAcademicTerms = 4
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13886
--Zaq Humphrey
update funding 
set TotalAcademicTerms = 6
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
