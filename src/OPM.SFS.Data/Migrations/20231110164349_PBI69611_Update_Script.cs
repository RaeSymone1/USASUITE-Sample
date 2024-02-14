using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class PBI69611UpdateScript : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string script = @"--Russell Griffith
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 79
--Ryan Leigland
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 96
--Samantha Fields
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 128
--Sonali Ubhayakar
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 210
--Sara Wiley
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 342
--Theodore Stehney
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 464
--Colin OSullivan
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 467
--Kenneth Wijesinghe
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 816
--John Trifiletti
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1019
--James Doyle
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1022
--Kristina Cole
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1236
--Chamel Evans
update funding 
set EnrolledSession = 'Fall '
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1475
--Mike Cook
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1519
--Laura Brown
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1531
--Kyle OMeara
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1577
--Samuel Clements
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1582
--Emily Ecoff
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1872
--Chad Johnson
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1894
--Yohann Young - Sumner
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 1912
--Matthew Lempka
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2156
--William Van Besien
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2172
--Erik Beckstrom
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2197
--Jonathan Leigh
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2563
--John Doyle
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2576
--Adrien Cheval
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2584
--Celia Paulsen
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2672
--Jill Rowland
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2937
--Michael DAngelo
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2941
--Ryan Stepanek
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2959
--Matthew Fillette
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2960
--Brittany De Bella
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 2988
--Benjamin VanPelt
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3033
--Zachary Otto
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3059
--Bilan Jones
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3177
--Jessica Owzarski
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3293
--Kelley Raines
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3376
--BENJAMIN NKRUMAH
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3499
--Cindy Martinez
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3542
--Hans Vargas Silva
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3558
--Mauranda Elliott
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3560
--Thomas McCarthy
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3680
--Alex Doyal
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3755
--Ashley Ragland
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3765
--Kelly Hood
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3792
--Benjamin Schmidt
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3877
--Shauna Policicchio
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3917
--Kristina Heinricy
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 3926
--Hector Garcia-Lomeli
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4001
--Bryan Conner
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4029
--Michael Cook
update funding 
set EnrolledSession = 'Fall '
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4063
--Ryan Smith
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4135
--Sarah Yoder
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4206
--Arthur Jicha
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4219
--Stacey Dunson
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4291
--Corey Mize
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4306
--Derek Smith
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4307
--Caroline Holcomb
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4309
--Michael Thompson
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4319
--Allison Holt
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4323
--Nicholas Schuchhardt
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4367
--James Williams
update funding 
set EnrolledSession = 'Fall '
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4385
--Marcellus Williams
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4391
--Kathryn Johnston
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4423
--Whitney Merrill
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4424
--Bridget Pelletier-Ross
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4484
--Kayla Mormak
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4494
--Miranda Hamilton
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4521
--William Morgan
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4573
--Shane Rogers
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4594
--Rahmira Rufus
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 4603
--Miriam Feldhausen
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5668
--Cervando Banuelos
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5671
--Joseph Wilson
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5773
--Nicholas Kantor
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5778
--Emily Leathers
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5785
--Shelby Anderson
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5841
--Sara Mitchell
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5871
--Jesus Carrete-Lopez
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5961
--Ricardo Castilla
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 5984
--Mario J Vazquez
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 6099
--Abrielle Holloway
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 6102
--David Afield
update funding 
set EnrolledSession = 'Summer'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 6144
--Dalton Brucker-Hahn
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 6166
--Shane Bennett
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7244
--Adrian Hy
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7246
--Joanna Hall
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7275
--Melissa Hannis
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7289
--Ronak Patel
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7316
--Megan Isaacs
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7457
--Marcus Brown
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7501
--Amber Paris
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7561
--LeTasha McFarlane-Garrard
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7663
--Terrence Kvitchko
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7711
--Michael Garippo
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7757
--Emily OHanlon
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7794
--Mbi Mbah Anchi
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7797
--Ming Chen
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7801
--Rachael Dunn
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7855
--JoHannah Couture
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7859
--Tempestt Neal
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7861
--Lauren Good
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7968
--Evan White
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 7982
--MaryAnn VanValkenburg
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 8026
--Kristen Aiken
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 8030
--George Harter
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 8064
--Michael Walker
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 8065
--Raymond Morain
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9354
--James Bell
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9428
--Colby Parker
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9431
--Mattisen Halliday
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9481
--Hannah Kleinheider
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9512
--Beckett Browning
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9537
--Lydia Speirs
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9545
--William Johnson
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9551
--Jennifer Guerra
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 9564
--Nuria Pacheco
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10788
--Travis Wright
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10799
--Ash Harper
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10872
--Blair Doyle
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10894
--Sophia Gatsios
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10927
--Mackenzie Oestreich
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10928
--Courtney Grubbs
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 10941
--Yusef Yassin
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11022
--Felix Baez Merced
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11023
--Thomas McCullough
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11025
--Janine Willms Brasseaux
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11077
--Anna Opsahl
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11124
--Benjamin Hughes
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11277
--Alexandra Mattika
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11282
--Javier Franco
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11352
--Oscar Bautista Chia
update funding 
set EnrolledSession = 'Summer'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11483
--Joseph Corona Vazquez
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11572
--Skylar Rumpca
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11585
--Richard Nelson
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11587
--Carly Winters
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11752
--William Johnson
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11811
--Paul Duhe
update funding 
set EnrolledSession = 'Winter'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11828
--Henry Motta
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 11960
--Bradley Schlauder
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 12057
--Leojaris Brujan
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 12114
--John Melton
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13148
--Evan White
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13223
--Valrie Jules
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13389
--Matthew Jones
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13474
--Samuel Dekemper
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13684
--Andrew Turner
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13706
--Dino Bonaldo
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13739
--Charlotte Negron Negron Lopez De Vic
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13751
--Eduard Ramos
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13755
--Jonathan Sosa
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13757
--Danay Rumbaut
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13765
--Daniel Daszkiewicz
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13768
--Tyler Thomas
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13778
--Allan Gonzalez Benjume
update funding 
set EnrolledSession = 'Fall'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13806
--Diamaris Gonz�lez-Rodr�guez
update funding 
set EnrolledSession = 'Winter'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13837
--Aaron Brown
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13844
--Vincent Pisani
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13871
--Sankofa Benzo
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13884
--Jessica Berrios
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13885
--William Fritts
update funding 
set EnrolledSession = 'Summer'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13886
--Gregory Turnberg
update funding 
set EnrolledSession = 'Spring'
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID 
where s.StudentUID = 13889
--Zaq Humphrey
update funding 
set EnrolledSession = 'Fall'
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
