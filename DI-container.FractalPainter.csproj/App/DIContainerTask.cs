using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using FractalPainting.App.Fractals;
using FractalPainting.Infrastructure.Common;
using FractalPainting.Infrastructure.UiActions;
using Ninject;
using Ninject.Extensions.Factory;
using Ninject.Extensions.Conventions;

namespace FractalPainting.App
{
    public static class DIContainerTask
    {
        public static MainForm CreateMainForm() => ConfigureContainer().Get<MainForm>();

        public static StandardKernel ConfigureContainer()
        {
            var container = new StandardKernel();
            container.Bind<IObjectSerializer>().To<XmlObjectSerializer>().WhenInjectedInto<SettingsManager>(); ;
            container.Bind<IBlobStorage>().To<FileBlobStorage>().WhenInjectedInto<SettingsManager>(); ;
            container.Bind<AppSettings>()
                .ToMethod(context => context.Kernel.Get<SettingsManager>().Load())
                .InSingletonScope();
            container.Bind<ImageSettings>()
                .ToMethod(context => context.Kernel.Get<SettingsManager>().Load().ImageSettings)
                .InSingletonScope();
            container.Bind<Palette>().ToConstant(new Palette()).InSingletonScope();
            container.Bind<IImageHolder, PictureBoxImageHolder>().ToConstant(new PictureBoxImageHolder()).InSingletonScope();
            container.Bind(x => x.FromThisAssembly()
                .SelectAllClasses()
                .InheritedFrom<IUiAction>()
                .BindAllInterfaces());
            container.Bind<IDragonPainterFactory>().ToFactory();

            return container;
        }
    }

    public interface IDragonPainterFactory
    {
        DragonPainter CreateDragonPainter(DragonSettings settings);
    }

    public class DragonFractalAction : IUiAction
    {
        public MenuCategory Category => MenuCategory.Fractals;
        public string Name => "Дракон";
        public string Description => "Дракон Хартера-Хейтуэя";
        private IDragonPainterFactory DragonPainterFactory { get; set; }

        public DragonFractalAction(IDragonPainterFactory dragonPainterFactory)
        {
            DragonPainterFactory = dragonPainterFactory;
        }

        public void Perform()
        {
            var dragonSettings = CreateRandomSettings();
            SettingsForm.For(dragonSettings).ShowDialog();
            DragonPainterFactory.CreateDragonPainter(dragonSettings).Paint();
        }

        private static DragonSettings CreateRandomSettings()
        {
            return new DragonSettingsGenerator(new Random()).Generate();
        }
    }

    public class KochFractalAction : IUiAction
    {
        public MenuCategory Category => MenuCategory.Fractals;
        public string Name => "Кривая Коха";
        public string Description => "Кривая Коха";
        private Lazy<KochPainter> KochPainter { get; set; }

        public KochFractalAction(Lazy<KochPainter> kochPainter)
        {
            KochPainter = kochPainter;
        }

        public void Perform()
        {
            KochPainter.Value.Paint();
        }
    }

    public class DragonPainter
    {
        private readonly IImageHolder imageHolder;
        private readonly DragonSettings settings;
        private Palette palette;

        public DragonPainter(IImageHolder imageHolder, DragonSettings settings, Palette palette)
        {
            this.imageHolder = imageHolder;
            this.settings = settings;
            this.palette = palette;
        }

        public void Paint()
        {
            var imageSize = imageHolder.GetImageSize();
            var size = Math.Min(imageSize.Width, imageSize.Height) / 2.1f;
            using (var graphics = imageHolder.StartDrawing())
            {
                using (var backgroundBrush = new SolidBrush(palette.BackgroundColor))
                    graphics.FillRectangle(backgroundBrush, 0, 0, imageSize.Width, imageSize.Height);
                var r = new Random();
                var cosa = (float)Math.Cos(settings.Angle1);
                var sina = (float)Math.Sin(settings.Angle1);
                var cosb = (float)Math.Cos(settings.Angle2);
                var sinb = (float)Math.Sin(settings.Angle2);
                var shiftX = settings.ShiftX * size * 0.8f;
                var shiftY = settings.ShiftY * size * 0.8f;
                var scale = settings.Scale;
                var p = new PointF(0, 0);
                foreach (var i in Enumerable.Range(0, settings.IterationsCount))
                {
                    using (var primaryBrush = new SolidBrush(palette.PrimaryColor))
                        graphics.FillRectangle(primaryBrush, imageSize.Width / 3f + p.X, imageSize.Height / 2f + p.Y, 1, 1);
                    if (r.Next(0, 2) == 0)
                        p = new PointF(scale * (p.X * cosa - p.Y * sina), scale * (p.X * sina + p.Y * cosa));
                    else
                        p = new PointF(scale * (p.X * cosb - p.Y * sinb) + shiftX, scale * (p.X * sinb + p.Y * cosb) + shiftY);
                    if (i % 100 == 0) imageHolder.UpdateUi();
                }
            }
            imageHolder.UpdateUi();
        }
    }
}