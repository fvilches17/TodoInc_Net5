using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using TodoInc.Client.DomModels;

namespace TodoInc.Client.Pages
{
    public partial class JotItDown
    {
        private ElementReference _canvasWrapperElement;
        private Canvas2DContext _canvas2DContext;
        protected BECanvasComponent CanvasReference;
        private BoundingClientRect _canvasBoundingClientRect;
        private bool _isMouseClickerDown;
        private double? _lastX;
        private double? _lastY;
        private const int CanvasWidth = 350;
        private const int CanvasHeight = 450;
        private const int PointSize = 2;
        private const int LineWidth = 2 * PointSize;
        private const double Radius = Math.PI * 2;

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _canvas2DContext = await CanvasReference.CreateCanvas2DAsync();
                _canvasBoundingClientRect = await JsRuntime.InvokeAsync<BoundingClientRect>("TodoIncJsInteropHelper.getBoundingClientRect", _canvasWrapperElement);
            }
        }

        public void SetMouseDownState() => _isMouseClickerDown = true;
        
        public async Task UpdateMouseCoordinatesAsync(MouseEventArgs e)
        {
            if (!_isMouseClickerDown) return;
            await TraceLineAsync(e.OffsetX, e.OffsetY);
        }

        public async Task UpdateTouchCoordinates(TouchEventArgs e)
        {
            var x = e.Touches[0].ClientX - _canvasBoundingClientRect.Left;
            var y = e.Touches[0].ClientY - _canvasBoundingClientRect.Top;
            await TraceLineAsync(x, y);
        }

        private async Task TraceLineAsync(double x, double y)
        {
            if (_lastX is not null && _lastY is not null)
            {
                await _canvas2DContext.SetLineWidthAsync(LineWidth);
                await _canvas2DContext.BeginPathAsync();
                await _canvas2DContext.MoveToAsync(_lastX.Value, _lastY.Value);
                await _canvas2DContext.LineToAsync(x, y);
                await _canvas2DContext.StrokeAsync();
            }

            await _canvas2DContext.BeginPathAsync();
            await _canvas2DContext.ArcAsync(x, y, PointSize, 0, Radius, anticlockwise: true);
            await _canvas2DContext.ClosePathAsync();
            await _canvas2DContext.FillAsync();

            _lastX = x;
            _lastY = y;
        }

        private async Task ClearCanvasAsync()
        {
            await _canvas2DContext.ClearRectAsync(0, 0, CanvasWidth, CanvasHeight);
            ResetState();
        }

        protected void ResetState()
        {
            _lastX = null;
            _lastY = null;
            _isMouseClickerDown = false;
        }
    }
}
